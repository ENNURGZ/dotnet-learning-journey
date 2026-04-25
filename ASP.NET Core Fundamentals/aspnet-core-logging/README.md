# Order Processing: ASP.NET Core Logging

Intermediate level task for practicing ASP.NET Core Logging patterns, structured logging, log levels, and logging configuration.

Estimated time to complete the task: **1 hour**.

The task requires .NET 8 SDK installed.

## Prerequisites

**Important:** This task requires you to use your completed solution from the previous assignments:
- "Order Processing: Dependency Injection в ASP.NET Core"
- "Order Processing: ASP.NET Core Routing"
- "Order Processing: ASP.NET Core Error Handling"
- "Order Processing: ASP.NET Core Configuration"
- "Order Processing: ASP.NET Core Environments"

## Task Description

Extend the existing `OrderProcessingWithDi` project by implementing comprehensive logging throughout the application. You will add structured logging to services and middleware, configure log levels, demonstrate different logging patterns, and create logging demo endpoints.

All logging should be added without modifying existing endpoint implementations (to maintain backward compatibility with existing tests).

### Project Structure

```
OrderProcessingWithDi/
├── Services/
│   └── Implementations/
│       ├── OrderService.cs                    # TODO: Add ILogger<T> and structured logging
│       ├── OrderValidator.cs                  # TODO: Add ILogger<T> and logging
│       └── PricingService.cs                 # TODO: Add ILogger<T> and logging
├── Middleware/
│   └── ErrorHandlingMiddleware.cs             # TODO: Enhance existing logging
└── Program.cs                                 # TODO: Configure logging and add logging endpoints
```

## Task Requirements

### 1. Add Logging to Services

Add structured logging to service classes using `ILogger<T>`.

#### OrderService

**Requirements:**
- Add constructor parameter: `ILogger<OrderService> logger`
- Store logger in a private readonly field
- Add logging in `ProcessOrderAsync`:
  * Log Information when order processing starts: `"Processing order for product {ProductId} with quantity {Quantity}"`
  * Log Warning when validation fails: `"Order validation failed: {ErrorMessage}"`
  * Log Information when order is successfully processed: `"Order processed successfully. ProductId: {ProductId}, Total: {Total}"`
  * Log Error when exception occurs: `"Error processing order: {ErrorMessage}"`

**Hint:** Use structured logging with parameters: `logger.LogInformation("Processing order for product {ProductId} with quantity {Quantity}", productId, quantity)`

#### OrderValidator

**Requirements:**
- Add constructor parameter: `ILogger<OrderValidator> logger`
- Store logger in a private readonly field
- Add logging in `Validate`:
  * Log Debug when validation starts: `"Validating order: ProductId={ProductId}, Quantity={Quantity}, UnitPrice={UnitPrice}"`
  * Log Warning when validation fails: `"Validation failed: {ValidationMessage}"`
  * Log Debug when validation succeeds: `"Validation passed for product {ProductId}"`

#### PricingService

**Requirements:**
- Add constructor parameter: `ILogger<PricingService> logger`
- Store logger in a private readonly field
- Add logging in `CalculateTotal`:
  * Log Debug when calculation starts: `"Calculating total for price {Price} and quantity {Quantity}"`
  * Log Information when discount is applied: `"Discount applied. Original total: {OriginalTotal}, Discount: {DiscountPercentage}%, Final total: {FinalTotal}"`
  * Log Debug when discount is not applied: `"No discount applied. Total: {Total}"`

### 2. Configure Logging in appsettings.json

**TODO:** Update logging configuration in `appsettings.json`.

**Requirements:**
- Configure log levels for different namespaces:
  * `"Default"`: `"Information"`
  * `"Microsoft.AspNetCore"`: `"Warning"`
  * `"OrderProcessingWithDi.Services.Implementations.OrderService"`: `"Debug"`
  * `"OrderProcessingWithDi.Services.Implementations.PricingService"`: `"Debug"`

**Example:**
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning",
      "OrderProcessingWithDi.Services.Implementations.OrderService": "Debug",
      "OrderProcessingWithDi.Services.Implementations.PricingService": "Debug"
    }
  }
}
```

### 3. Create Logging Demo Endpoints

Create endpoints that demonstrate different logging scenarios and log levels.

#### GET `/logging/demo` - Logging Demo

Create an endpoint that demonstrates different log levels.

**Requirements:**
- Route: `/logging/demo`
- Inject `ILogger<Program>` via method injection
- Log messages with different levels:
  * `Trace`: `"This is a Trace message"`
  * `Debug`: `"This is a Debug message"`
  * `Information`: `"This is an Information message"`
  * `Warning`: `"This is a Warning message"`
  * `Error`: `"This is an Error message"`
  * `Critical`: `"This is a Critical message"`
- Return `Results.Ok` with message: `"Check application logs to see different log levels"`
- Add `.WithName("LoggingDemo")` and `.WithTags("Logging")`

#### GET `/logging/structured` - Structured Logging Demo

Create an endpoint that demonstrates structured logging with parameters.

**Requirements:**
- Route: `/logging/structured`
- Inject `ILogger<Program>` via method injection
- Log structured messages with parameters:
  * `Information`: `"Structured log example: UserId={UserId}, Action={Action}, Timestamp={Timestamp}"`
  * Use query parameters or generate sample values
- Return `Results.Ok` with the logged parameters
- Add `.WithName("StructuredLoggingDemo")` and `.WithTags("Logging")`

**Example:**
```csharp
var userId = "user123";
var action = "GetOrders";
var timestamp = DateTime.UtcNow;

logger.LogInformation("Structured log example: UserId={UserId}, Action={Action}, Timestamp={Timestamp}", 
    userId, action, timestamp);

return Results.Ok(new { userId, action, timestamp });
```

#### GET `/logging/scopes` - Logging Scopes Demo

Create an endpoint that demonstrates logging scopes.

**Requirements:**
- Route: `/logging/scopes`
- Inject `ILogger<Program>` via method injection
- Use `ILogger.BeginScope()` to create logging scopes:
  * Outer scope: `"RequestId: {RequestId}"`
  * Inner scope: `"Operation: {Operation}"`
  * Log messages within scopes
- Return `Results.Ok` with message
- Add `.WithName("LoggingScopesDemo")` and `.WithTags("Logging")`

**Example:**
```csharp
using (logger.BeginScope("RequestId: {RequestId}", Guid.NewGuid()))
{
    logger.LogInformation("Processing request");
    
    using (logger.BeginScope("Operation: {Operation}", "GetOrders"))
    {
        logger.LogInformation("Executing operation");
    }
    
    logger.LogInformation("Request completed");
}

return Results.Ok(new { message = "Check logs for scoped logging demonstration" });
```

### 4. Enhance ErrorHandlingMiddleware Logging

Update `ErrorHandlingMiddleware` to include more detailed logging.

**Requirements:**
- Add structured logging in `HandleExceptionAsync`:
  * Log Error with exception details: `"Exception handled: {ExceptionType}, Message: {Message}, Path: {Path}"`
  * Include exception type, message, and request path in structured parameters

## Testing

The project includes a test suite using **xUnit** framework. Test methods are marked with the `[Fact]` attribute.

**Note:** The test project is configured with global using directives:
- `Xunit` - allows using `[Fact]` attribute without explicit `using Xunit;`
- `Moq` - allows using `Mock<T>` without explicit `using Moq;`

This means you can use `[Fact]` and `Mock<T>` directly in test files without adding using statements.

**Important:** Logging tests in `OrderProcessingWithDi.Tests/Services/` are initially skipped (marked with `[Fact(Skip = "...")]`) because logging is not yet implemented. The `[Fact]` attribute is required for xUnit to recognize test methods, but the `Skip` parameter prevents them from running until logging is implemented.

After implementing logging in the services, you need to activate the tests:

1. **Remove the `Skip` parameter** from all test methods in:
   - `OrderServiceLoggingTests.cs`
   - `OrderValidatorLoggingTests.cs`
   - `PricingServiceLoggingTests.cs`
   
   Change `[Fact(Skip = "...")]` to `[Fact]` - keep the `[Fact]` attribute, but remove the `Skip` parameter so tests will run.

2. Update constructor calls in test setup to include `ILogger<T>` parameters

3. Uncomment the `Verify()` calls in the test methods to check that logging is working correctly

4. Run `dotnet test` to verify all logging tests pass

## Validation

1. Run `dotnet build` at the solution root. The project should build without errors.
2. Run `dotnet test` to execute all unit and integration tests. All tests should pass.
3. Start the application with `dotnet run --project OrderProcessingWithDi`.

**Note:** The project includes an `api.http` file in the `OrderProcessingWithDi` folder with pre-configured HTTP requests for testing all endpoints. You can use it with REST Client extensions:
- **VS Code:** [REST Client](https://marketplace.visualstudio.com/items?itemName=humao.rest-client)
- **JetBrains Rider:** Built-in support (no extension needed)
- **Visual Studio:** [REST Client](https://marketplace.visualstudio.com/items?itemName=MadsKristensen.RestClient) or [REST Client File Handler](https://marketplace.visualstudio.com/items?itemName=humao.rest-client)

4. Test the logging endpoints:

   **Logging Demo:**
   ```bash
   curl http://localhost:5000/logging/demo
   ```
   Expected: Check console output for different log levels.

   **Structured Logging:**
   ```bash
   curl http://localhost:5000/logging/structured
   ```
   Expected: Structured log entries with parameters in console.

   **Logging Scopes:**
   ```bash
   curl http://localhost:5000/logging/scopes
   ```
   Expected: Log entries with scopes in console.

5. Test service logging by creating an order:
   ```bash
   curl -X POST "http://localhost:5000/orders?productId=A1&quantity=6&unitPrice=10"
   ```
   Expected: Check console for logging from OrderService, OrderValidator, and PricingService.

6. Test different log levels by changing configuration in `appsettings.json`:
   * Set `"Default"` to `"Debug"` - should see Debug messages
   * Set `"Default"` to `"Warning"` - should only see Warning and above

## References

- [ASP.NET Core Logging](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/logging/)
- [Structured Logging](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/logging/#log-message-template)
- [Log Levels](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/logging/#log-levels)
- [Logging Scopes](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/logging/#log-scopes)