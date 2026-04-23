# Middleware Library

Intermediate level task for practicing middleware development and extension method implementation in ASP.NET Core.

Estimated time to complete the task: 2 hours.

The task requires .NET 8 SDK installed.

## Task Description

All public methods in the `MiddlewareLibrary` project are stubbed with `throw new NotImplementedException()`. Restore the working behavior of every middleware component and all extension methods.

### Project Structure

```
MiddlewareLibrary/
 ├── Extensions/
 │   └── MiddlewareExtensions.cs
 └── Middleware/
     ├── AuthenticationMiddleware.cs
     ├── CachingMiddleware.cs
     ├── CompressionMiddleware.cs
     ├── ConfigurableMiddleware.cs
     ├── ErrorHandlingMiddleware.cs
     ├── RateLimitingMiddleware.cs
     └── RequestLoggingMiddleware.cs
```

### MiddlewareExtensions.cs

Add implementations of the extension methods that register middleware components:

* `UseRequestLogging`
* `UseErrorHandling`
* `UseConfigurableMiddleware`
* `UseApiAuthentication`
* `UseCustomCompression`
* `UseCustomCaching`
* `UseCustomRateLimiting`

Each method must call `UseMiddleware<TMiddleware>(...)`. When options are required, instantiate the options class, apply the configuration delegate, and pass the configured options to `UseMiddleware`.

### RequestLoggingMiddleware

Middleware responsible for logging incoming requests.

* Constructor parameters: `RequestDelegate next`, `ILogger<RequestLoggingMiddleware> logger`.
* `InvokeAsync(HttpContext context)` must:
  * Capture `startTime = DateTime.UtcNow`.
  * Generate `requestId = Guid.NewGuid().ToString("N")[..8]`.
  * Set `context.Response.Headers["X-Request-ID"] = requestId`.
  * Log start of processing using  
    `logger.LogInformation("[{RequestId}] Starting request processing: {Method} {Path}", ...)`.
  * Call `await next(context)`.
  * After the next middleware completes, compute request duration and log completion using  
    `logger.LogInformation("[{RequestId}] Request processing completed: {StatusCode} in {Duration}ms", ...)`.

### ErrorHandlingMiddleware

Middleware responsible for handling unhandled exceptions and producing a consistent error response.

* Constructor parameters: `RequestDelegate next`, `ILogger<ErrorHandlingMiddleware> logger`.
* `InvokeAsync(HttpContext context)` must:
  * Execute `await next(context)` inside a `try` block.
  * Catch `Exception ex`, log the error via  
    `logger.LogError(ex, "An error occurred while processing request {Path}", context.Request.Path);`,
    and call `await HandleExceptionAsync(context, ex)`.
* `HandleExceptionAsync(HttpContext context, Exception exception)` must:
  * Set `context.Response.StatusCode` to 400 for `ArgumentNullException`, 401 for `UnauthorizedAccessException`, 501 for `NotImplementedException`, otherwise 500.
  * Set `context.Response.ContentType = MediaTypeNames.Application.Json`.
  * Serialize `{ error, message, statusCode }` via `JsonSerializer.Serialize` and write it to the response body.

### ConfigurableMiddleware

Middleware that adapts its behavior based on options.

* Constructor parameters: `RequestDelegate next`, `ConfigurableMiddlewareOptions options`.
* `InvokeAsync(HttpContext context)` must:
  * When `options.EnableLogging` is `true`, resolve `ILogger<ConfigurableMiddleware>` from `context.RequestServices` and log the current path.
  * When `options.EnableCustomHeader` is `true`, set `context.Response.Headers["X-Custom-Header"]` to `options.CustomHeaderValue`.
  * Call `await next(context)`.
* `ConfigurableMiddlewareOptions` defaults:
  * `EnableLogging = true`.
  * `EnableCustomHeader = false`.
  * `CustomHeaderValue = "Custom Value"`.

### AuthenticationMiddleware

Middleware responsible for enforcing API-key based authentication.

* Constructor parameters: `RequestDelegate next`, `ILogger<AuthenticationMiddleware> logger`, `IConfiguration configuration`.
* `InvokeAsync(HttpContext context)` must:
  * Skip authentication by calling `await next(context)` when `ShouldSkipAuthentication(context.Request.Path)` returns `true`.
  * Read the API key from the `X-API-Key` header. If missing, log a warning and call `ReturnUnauthorized(context, "API key not provided")`.
  * Compare the provided key with `configuration["ApiKey"]`. If the values do not match, log a warning and return 401 with the message `"Invalid API key"`.
  * Log successful authentication and call `await next(context)` when validation succeeds.
* `ShouldSkipAuthentication(PathString path)` must return `true` for paths starting with `/`, `/health`, or `/swagger` (use `path.StartsWithSegments`).
* `ReturnUnauthorized(HttpContext context, string message)` must:
  * Set the response status code to 401 and `ContentType` to `"application/json"`.
  * Produce a JSON response `{ error = "Authentication error", message, timestamp = DateTime.UtcNow }` via `WriteAsJsonAsync`.

### CompressionMiddleware

Middleware responsible for compressing responses when the client supports it.

* Constructor parameters: `RequestDelegate next`, `ILogger<CompressionMiddleware> logger`.
* `InvokeAsync(HttpContext context)` must:
  * Store the original `context.Response.Body`.
  * Inspect the `Accept-Encoding` header (case-insensitive).
  * When the header contains `"gzip"`, set `Content-Encoding = "gzip"` and wrap the response body with `new GZipStream(originalBodyStream, CompressionLevel.Fastest, leaveOpen: true)`.
  * When the header contains `"deflate"`, perform the same flow with `DeflateStream`.
  * Call `await next(context)`.
  * In `finally`, if the current response body is a compression stream, dispose it asynchronously, restore the original body stream, and log  
    `logger.LogDebug("Response stream compressed for {Path}", context.Request.Path);`.

### CachingMiddleware

Middleware responsible for caching responses for configured paths.

* Constructor parameters: `RequestDelegate next`, `ILogger<CachingMiddleware> logger`, `CachingOptions options`.
* Maintains a static `ConcurrentDictionary<string, CacheEntry> Cache`.
* `InvokeAsync(HttpContext context)` must:
  * Call `await next(context)` immediately when `ShouldCache(context.Request.Path)` is `false`.
  * Generate the cache key using `"{Method}:{Path}:{QueryString}"`.
  * When `TryGetFromCache` returns `true`, log `"Cache HIT for {Path}"` and serve the stored response via `WriteCachedResponse`.
  * When the cache misses:
    * Log `"Cache MISS for {Path}"`.
    * Swap `context.Response.Body` with a `MemoryStream`.
    * Invoke `await next(context)`.
    * Read the buffered response, create a `CacheEntry` with content, `ContentType`, `StatusCode`, and expiration `DateTime.UtcNow.AddMinutes(options.CacheDurationMinutes)`.
    * Store the entry via `SaveToCache`.
    * Add cache headers via `AddCacheHeaders`.
    * Copy the buffered response back to the original stream and restore `Response.Body`.
* Helper methods must:
  * `ShouldCache` — return `true` only when `options.CacheablePaths` contains a prefix that matches the current request path.
  * `TryGetFromCache` — return `true` when a valid (non-expired) entry exists; remove expired entries.
  * `SaveToCache` — insert or replace the dictionary entry.
  * `AddCacheHeaders` — set `Cache-Control` and generate an `ETag` using `Guid.NewGuid()`.
  * `WriteCachedResponse` — restore `StatusCode`, `ContentType`, apply cache headers, and write the cached body.
* `CachingOptions` defaults:
  * `CacheDurationMinutes = 60`.
  * `CacheablePaths = Array.Empty<string>()`.

### RateLimitingMiddleware

Middleware that throttles requests based on client IP.

* Constructor parameters: `RequestDelegate next`, `ILogger<RateLimitingMiddleware> logger`, `RateLimitingOptions options`.
* Uses a static `ConcurrentDictionary<string, RequestLog>` where `RequestLog` contains `List<DateTime> Timestamps` and a synchronization `Lock`.
* `InvokeAsync(HttpContext context)` must:
  * Skip rate limiting when `ShouldSkip(context.Request.Path)` returns `true`.
  * Determine `clientIp = context.Connection.RemoteIpAddress?.ToString() ?? "unknown"`.
  * Call `IsRequestAllowed(clientIp)`. If it returns `false`, log a warning and respond via `ReturnRateLimitExceeded(context)` (HTTP 429, `Retry-After = "60"`, JSON payload with `retryAfter = 60`).
  * When allowed, continue the pipeline with `await next(context)`.
* `ShouldSkip(PathString path)` — return `true` if the path starts with any value from `options.ExcludedPaths`.
* `IsRequestAllowed(string clientIp)` must:
  * Compute `now = DateTime.UtcNow` and `windowStart = now.AddMinutes(-options.WindowMinutes)`.
  * Retrieve or create the `RequestLog` entry. Inside a `lock`, remove timestamps older than `windowStart`.
  * If the number of remaining timestamps is greater than or equal to `options.MaxRequests`, return `false`; otherwise, add the current timestamp and return `true`.
* `ReturnRateLimitExceeded(HttpContext context)` must set status 429, header `Retry-After = "60"`, content type `application/json`, and write a JSON response explaining the limit.
* `RateLimitingOptions` defaults:
  * `MaxRequests = 100`.
  * `WindowMinutes = 1`.
  * `ExcludedPaths = Array.Empty<string>()`.

