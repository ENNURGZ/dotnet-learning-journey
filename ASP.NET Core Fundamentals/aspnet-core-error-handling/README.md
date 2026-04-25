# Order Processing: ASP.NET Core Error Handling

Intermediate level task for practicing ASP.NET Core Error Handling patterns, exception middleware, and standardized error responses.

Estimated time to complete the task: **2 hours**.

The task requires .NET 8 SDK installed.

## Prerequisites

**Important:** This task requires you to use your completed solution from the previous assignments:
- "Order Processing: Dependency Injection в ASP.NET Core"
- "Order Processing: ASP.NET Core Routing"

## Task Description

Extend the existing `OrderProcessingWithDi` project by implementing comprehensive error handling. You will add custom exceptions, global exception handling middleware, and standardized error responses following RFC 7807 Problem Details.

All error handling should be added without modifying existing endpoint implementations (to maintain backward compatibility with existing tests).

### Project Structure

```
OrderProcessingWithDi/
├── Models/
│   ├── ErrorResponse.cs                        # TODO: Create error response model
│   └── Exceptions/
│       ├── OrderNotFoundException.cs           # TODO: Create custom exception
│       └── InvalidOrderException.cs            # TODO: Create custom exception
├── Middleware/
│   └── ErrorHandlingMiddleware.cs              # TODO: Create error handling middleware
└── Program.cs                                   # TODO: Register error handling middleware
```

## Task Requirements

### 1. Create Custom Exceptions

#### OrderNotFoundException

Create a custom exception for when an order is not found:

```csharp
public class OrderNotFoundException : Exception
{
    public int OrderId { get; }
    
    public OrderNotFoundException(int orderId)
        : base($"Order with ID {orderId} was not found.")
    {
        this.OrderId = orderId;
    }
    
    // Include standard Exception constructors for CA1032 compliance
}
```

**Requirements:**
- Inherit from `Exception`
- Store the `orderId` that was not found
- Include standard constructors: `()`, `(string message)`, `(string message, Exception innerException)`

#### InvalidOrderException

Create a custom exception for order validation failures:

```csharp
public class InvalidOrderException : Exception
{
    public InvalidOrderException(string message)
        : base(message)
    {
    }
    
    // Include standard Exception constructors for CA1032 compliance
}
```

**Requirements:**
- Inherit from `Exception`
- Accept validation error message
- Include standard constructors: `()`, `(string message)`, `(string message, Exception innerException)`

### 2. Create Error Response Model

Create an `ErrorResponse` class following RFC 7807 Problem Details:

```csharp
public class ErrorResponse
{
    public string Type { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public int Status { get; set; }
    public string Detail { get; set; } = string.Empty;
    public string? Instance { get; set; }
    public Dictionary<string, object> Extensions { get; set; } = new();
}
```

**Fields:**
- `Type`: URI reference identifying the problem type (RFC 7231 section references)
- `Title`: Short summary of the problem
- `Status`: HTTP status code
- `Detail`: Detailed explanation of the problem
- `Instance`: URI reference identifying the specific occurrence
- `Extensions`: Additional error information (e.g., `orderId` for `OrderNotFoundException`)

### 3. Create Error Handling Middleware

Create `ErrorHandlingMiddleware` that:

1. Catches all exceptions in the request pipeline
2. Converts exceptions to `ErrorResponse` objects
3. Returns JSON responses with appropriate HTTP status codes
4. Logs errors using `ILogger`

**Exception Mapping:**

| Exception Type | HTTP Status | Type URI | Title |
|---------------|-------------|----------|-------|
| `OrderNotFoundException` | 404 | `rfc7231#section-6.5.4` | "Order Not Found" |
| `InvalidOrderException` | 400 | `rfc7231#section-6.5.1` | "Invalid Order" |
| `ArgumentException` | 400 | `rfc7231#section-6.5.1` | "Invalid Argument" |
| Other exceptions | 500 | `rfc7231#section-6.6.1` | "Internal Server Error" |

**Requirements:**
- Use `switch expression` to map exceptions to error responses
- Include `orderId` in `Extensions` for `OrderNotFoundException`
- Set `Instance` to `context.Request.Path`
- Serialize response as JSON with camelCase naming
- Log errors with appropriate log level

### 4. Update OrderService

Update `OrderService.ValidateOrder` to throw `InvalidOrderException` instead of `ArgumentException`:

```csharp
if (!validationResult.IsValid)
{
    throw new InvalidOrderException(validationResult.ErrorMessage ?? "Order validation failed");
}
```

### 5. Update Endpoints

Update the `GET /api/v1/orders/{orderId:int}` endpoint to throw `OrderNotFoundException`:

```csharp
ordersGroup.MapGet("/{orderId:int}", (int orderId, IOrderRepository repository) =>
{
    var order = repository.GetById(orderId);
    if (order == null)
    {
        throw new OrderNotFoundException(orderId);
    }
    return Results.Ok(order);
})
```

### 6. Register Middleware

Register the error handling middleware in `Program.cs` **BEFORE** other middleware:

```csharp
var app = builder.Build();

// Register error handling middleware FIRST
app.UseErrorHandling();

// Then register other middleware
app.UseDependencyInjectionDemo();
```

**Important:** Error handling middleware must be registered early in the pipeline to catch all exceptions.

## Requirements

* Create custom exception classes (`OrderNotFoundException`, `InvalidOrderException`)
* Create `ErrorResponse` model following RFC 7807
* Implement global error handling middleware
* Map exceptions to appropriate HTTP status codes
* Return standardized JSON error responses
* Log errors appropriately
* Update `OrderService` to use `InvalidOrderException`
* Update endpoints to throw `OrderNotFoundException`
* Register middleware in correct order
* Maintain backward compatibility with existing tests

## Important Notes for Automated Testing

**This assignment will be automatically tested on autocode.epam.com platform.**

To ensure your solution passes automated tests, please follow these requirements:

1. **Exception Names:** Use exact exception class names (`OrderNotFoundException`, `InvalidOrderException`)
2. **HTTP Status Codes:** Return correct status codes (400, 404, 500)
3. **Error Response Format:** Follow RFC 7807 Problem Details structure
4. **Middleware Registration:** Register `UseErrorHandling()` before other middleware
5. **Property Names:** Use camelCase for JSON serialization (`orderId`, not `OrderId`)

## Validation

1. Run `dotnet build` at the solution root. The project should build without errors.
2. Run `dotnet test` to execute all unit and integration tests. All tests should pass.
3. Start the application with `dotnet run --project OrderProcessingWithDi`.
4. Test error scenarios:

   **Order Not Found (404):**
   ```bash
   curl http://localhost:5000/api/v1/orders/999
   ```
   Expected response:
   ```json
   {
     "type": "https://tools.ietf.org/html/rfc7231#section-6.5.4",
     "title": "Order Not Found",
     "status": 404,
     "detail": "Order with ID 999 was not found.",
     "instance": "/api/v1/orders/999",
     "extensions": {
       "orderId": 999
     }
   }
   ```

   **Invalid Order (400):**
   ```bash
   curl -X POST "http://localhost:5000/orders?productId=&quantity=5&unitPrice=10"
   ```
   Expected response:
   ```json
   {
     "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
     "title": "Invalid Order",
     "status": 400,
     "detail": "ProductId cannot be empty",
     "instance": "/orders",
     "extensions": {}
   }
   ```

   **Invalid Argument (400):**
   ```bash
   curl -X POST "http://localhost:5000/orders?productId=A1&quantity=-5&unitPrice=10"
   ```
   Expected response:
   ```json
   {
     "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
     "title": "Invalid Argument",
     "status": 400,
     "detail": "Quantity must be greater than 0",
     "instance": "/orders",
     "extensions": {}
   }
   ```

## Expected Response Formats

**Order Not Found (404):**
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.4",
  "title": "Order Not Found",
  "status": 404,
  "detail": "Order with ID 999 was not found.",
  "instance": "/api/v1/orders/999",
  "extensions": {
    "orderId": 999
  }
}
```

**Invalid Order (400):**
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "title": "Invalid Order",
  "status": 400,
  "detail": "ProductId cannot be empty",
  "instance": "/orders",
  "extensions": {}
}
```

**Internal Server Error (500):**
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.6.1",
  "title": "Internal Server Error",
  "status": 500,
  "detail": "An unexpected error occurred while processing your request.",
  "instance": "/api/v1/orders/stats",
  "extensions": {}
}
```

## References

- [RFC 7807 - Problem Details for HTTP APIs](https://tools.ietf.org/html/rfc7807)
- [ASP.NET Core Middleware](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/middleware/)
- [Exception Handling in ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/error-handling)
