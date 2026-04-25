# Order Processing: Dependency Injection в ASP.NET Core

Intermediate level task for practicing Dependency Injection (DI) patterns and service lifetimes in ASP.NET Core.

Estimated time to complete the task: 3 hours.

The task requires .NET 8 SDK installed.

## Task Description

All service implementations and API endpoints in the `OrderProcessingWithDi` project are stubbed with `throw new NotImplementedException()`. Implement all service classes, register them with correct lifetimes, and create API endpoints using method injection.

### Project Structure

```
OrderProcessingWithDi/
├── Models/
│   ├── Order.cs                                # Order model (already implemented)
│   └── OrderResult.cs                          # Order processing result (already implemented)
├── Services/
│   ├── Interfaces/                             # All interfaces are already implemented
│   │   ├── IOrderRepository.cs
│   │   ├── IOrderService.cs
│   │   ├── IPricingService.cs
│   │   ├── IPricingServiceFactory.cs
│   │   ├── IOrderValidator.cs
│   │   └── ILifetimeDemoService.cs
│   └── Implementations/
│       ├── InMemoryOrderRepository.cs          # Already implemented (example)
│       ├── SimplePricingService.cs             # Already implemented (example)
│       ├── LifetimeDemoService.cs              # Already implemented (example)
│       ├── OrderService.cs                     # TODO: Implement
│       ├── OrderValidator.cs                   # TODO: Implement
│       ├── PricingService.cs                   # TODO: Implement
│       └── PricingServiceFactory.cs            # TODO: Implement
├── Middleware/
│   └── DependencyInjectionDemoMiddleware.cs    # Already implemented (example)
└── Program.cs                                  # TODO: Register services and implement endpoints
```

### OrderValidator
Service responsible for validating order parameters.

* Constructor: No dependencies required.
* `Validate(string productId, int quantity, decimal unitPrice)` must:
  * Return `(false, "ProductId cannot be empty")` when `productId` is `null` or empty (use `string.IsNullOrWhiteSpace()`).
  * Return `(false, "Quantity must be greater than 0")` when `quantity` is less than or equal to 0.
  * Return `(false, "UnitPrice must be greater than 0")` when `unitPrice` is less than or equal to 0.
  * Return `(true, null)` when all validations pass.

### PricingService
Service responsible for calculating order totals with discount support.

* Constructor: No dependencies required.
* Define constants:
  * `DiscountThreshold = 5` (quantity threshold for discount).
  * `DiscountPercentage = 0.1m` (10% discount).
  * `MinimumOrderValue = 0m` (minimum order value to apply discount).
* `CalculateTotal(decimal basePrice, int quantity)` must:
  * Calculate base total: `basePrice * quantity`.
  * When `quantity > DiscountThreshold` AND `total >= MinimumOrderValue`:
    apply discount: `total *= (1 - DiscountPercentage)`.
  * Return the final total.

**Example:** `basePrice = 10`, `quantity = 6` → base total `60`, discount applied → final total `54`.

### PricingServiceFactory
Factory implementation for creating pricing services based on type.

* Constructor parameters: `IServiceProvider serviceProvider`.
* Add a private field to store `IServiceProvider`.
* `CreatePricingService(string? serviceType = null)` must:
  * Set `serviceType` to `"standard"` when it is `null`.
  * Use `switch expression` to choose implementation:
    * `"standard"` → get `IPricingService` from `serviceProvider.GetRequiredService<IPricingService>()`.
    * `"simple"` → create a new instance of `SimplePricingService()`.
    * for unknown type → throw `ArgumentException` with message `"Unknown pricing service type: {serviceType}"`.

### OrderService
Service responsible for processing orders using multiple dependencies.

* Constructor parameters: `IPricingService pricingService`, `IOrderRepository repository`, `IOrderValidator validator`.
* Add private readonly fields for each dependency.
* `ProcessOrderAsync(string productId, int quantity, decimal unitPrice)` must:
  * Validate order using `validator.Validate(productId, quantity, unitPrice)`.
  * When validation fails (`IsValid == false`), throw `ArgumentException` with the error message from validation result.
  * Calculate total using `pricingService.CalculateTotal(unitPrice, quantity)`.
  * Create `OrderResult` with `productId`, `quantity`, and calculated `total`.
  * Save result via `await repository.SaveAsync(result)`.
  * Return `OrderResult`.

### Program.cs - Service Registration
Register all services with correct lifetimes in `Program.cs`:

* **Singleton:**
  * `IOrderRepository` as `InMemoryOrderRepository`.
  * `ILifetimeDemoService` as `LifetimeDemoService` (use factory: `sp => new LifetimeDemoService()`).

* **Scoped:**
  * `IOrderService` as `OrderService`.
  * `IOrderValidator` as `OrderValidator`.
  * `ILifetimeDemoService` as `LifetimeDemoService` (use factory: `sp => new LifetimeDemoService()`).

* **Transient:**
  * `IPricingService` as `PricingService`.
  * `ILifetimeDemoService` as `LifetimeDemoService` (use factory: `sp => new LifetimeDemoService()`).

* **Factory (Singleton):**
  * `IPricingServiceFactory` as `PricingServiceFactory`.

**Note:** `ILifetimeDemoService` is registered with all three lifetimes to demonstrate the difference. Each registration uses a factory delegate to create new instances.

### Middleware - DependencyInjectionDemoMiddleware

The `DependencyInjectionDemoMiddleware` demonstrates **constructor injection in middleware**. It shows how services with different lifetimes are injected into middleware components.

**Purpose:**
* Demonstrates DI in middleware (services injected via constructor)
* Adds response headers (`X-DI-Singleton-Instance`, `X-DI-Scoped-Instance`, `X-DI-Transient-Instance`) to visualize service lifetimes
* Works together with the `/di-demo` endpoint to show lifetime differences

**How it works:**
* Middleware receives three `ILifetimeDemoService` instances (one for each lifetime) via constructor injection
* For each request, it adds instance IDs to response headers
* You can check these headers to see how different lifetimes behave across multiple requests

**Note:** This middleware is already implemented. You need to register it in `Program.cs` using `app.UseDependencyInjectionDemo()`.

### Program.cs - API Endpoints
In Minimal API, services can be injected directly into route handlers via **method injection**. ASP.NET Core automatically resolves dependencies from the DI container.

#### POST `/orders` - Create Order

* Use `app.MapPost("/orders", ...)`.
* Handler signature: `async (string productId, int quantity, decimal unitPrice, IOrderService orderService) =>`.
* Call `await orderService.ProcessOrderAsync(productId, quantity, unitPrice)`.
* Return result via `Results.Ok(result)`.
* Add `.WithName("CreateOrder")` and `.WithTags("Orders")`.

#### GET `/orders` - Get All Orders

* Use `app.MapGet("/orders", ...)`.
* Handler signature: `(IOrderRepository repository) =>`.
* Call `repository.GetAll()`.
* Return result via `Results.Ok(orders)`.
* Add `.WithName("GetAllOrders")` and `.WithTags("Orders")`.

#### GET `/di-demo` - Demonstrate Service Lifetimes

* Use `app.MapGet("/di-demo", ...)`.
* Handler signature: `(ILifetimeDemoService singletonService, ILifetimeDemoService scopedService, ILifetimeDemoService transientService) =>`.
* ASP.NET Core will automatically resolve the three parameters with different lifetimes (Singleton, Scoped, Transient) based on registrations.
* Return JSON object with fields:
  * `message`: `"Dependency Injection Lifetime Demonstration"`.
  * `singleton`: object with `instanceId` and `createdAt` from `singletonService`.
  * `scoped`: object with `instanceId` and `createdAt` from `scopedService`.
  * `transient`: object with `instanceId` and `createdAt` from `transientService`.
  * `explanation`: object with explanations for each lifetime.
* Add `.WithName("DependencyInjectionDemo")` and `.WithTags("DI Demo")`.

**Note:** The `DependencyInjectionDemoMiddleware` adds response headers (`X-DI-Singleton-Instance`, `X-DI-Scoped-Instance`, `X-DI-Transient-Instance`) to all requests, including this endpoint. Check response headers to see how middleware receives services with different lifetimes.

#### GET `/factory-demo` - Demonstrate Factory Pattern

* Use `app.MapGet("/factory-demo", ...)`.
* Handler signature: `(IPricingServiceFactory factory, decimal price, int quantity, string? serviceType) =>`.
* Create `pricingService` via `factory.CreatePricingService(serviceType)`.
* Calculate `total` via `pricingService.CalculateTotal(price, quantity)`.
* Return JSON object with fields: `message`, `serviceType` (use `serviceType ?? "standard"`), `price`, `quantity`, `total`, `explanation`.
* Add `.WithName("FactoryDemo")` and `.WithTags("DI Demo")`.

## Requirements

* Remove every `throw new NotImplementedException()` in the listed files.
* Use constructor injection for service dependencies.
* Use method injection for Minimal API endpoints.
* Register services with correct lifetimes (Singleton, Scoped, Transient).
* Ensure proper error handling (throw `ArgumentException` for validation failures).
* Follow asynchronous patterns (`async`/`await`) where required.

## Validation

1. Run `dotnet build` at the solution root. The project should build without errors.
2. Run `dotnet test` to execute all unit and integration tests. All tests should pass.
3. Start the application with `dotnet run --project OrderProcessingWithDi`.
4. Test the API endpoints:

   **Create order (with discount):**
   ```bash
   curl -X POST "http://localhost:5000/orders?productId=A1&quantity=6&unitPrice=10"
   ```
   Expected result: `{"productId":"A1","quantity":6,"total":54}` (discount applied: 10% off for quantity > 5)

   **Create order (without discount):**
   ```bash
   curl -X POST "http://localhost:5000/orders?productId=B2&quantity=4&unitPrice=10"
   ```
   Expected result: `{"productId":"B2","quantity":4,"total":40}` (no discount: quantity <= 5)

   **Create order with validation errors:**
   ```bash
   # Empty productId
   curl -X POST "http://localhost:5000/orders?productId=&quantity=5&unitPrice=10"
   # Zero quantity
   curl -X POST "http://localhost:5000/orders?productId=C3&quantity=0&unitPrice=10"
   # Negative quantity
   curl -X POST "http://localhost:5000/orders?productId=C3&quantity=-5&unitPrice=10"
   # Zero unitPrice
   curl -X POST "http://localhost:5000/orders?productId=C3&quantity=5&unitPrice=0"
   # Negative unitPrice
   curl -X POST "http://localhost:5000/orders?productId=C3&quantity=5&unitPrice=-10"
   ```
   Expected: HTTP 500 Internal Server Error.
   Note: At this stage, error responses are not standardized; details may vary by environment.

   **Get all orders:**
   ```bash
   curl http://localhost:5000/orders
   ```
   Expected: JSON array with all created orders

   **Demonstrate DI lifetimes:**
   ```bash
   curl http://localhost:5000/di-demo
   ```
   Check response headers:
   ```bash
   curl -I http://localhost:5000/di-demo
   ```
   Look for headers: `X-DI-Singleton-Instance`, `X-DI-Scoped-Instance`, `X-DI-Transient-Instance`
   
   **Tip:** Make multiple requests to see that Singleton instance ID stays the same, while Scoped and Transient change.

   **Demonstrate Factory Pattern:**
   
   Standard service (applies discount):
   ```bash
   curl "http://localhost:5000/factory-demo?price=10&quantity=6&serviceType=standard"
   ```
   Expected: `{"total":54}` (discount applied: 10 * 6 * 0.9 = 54)
   
   Simple service (no discount):
   ```bash
   curl "http://localhost:5000/factory-demo?price=10&quantity=6&serviceType=simple"
   ```
   Expected: `{"total":60}` (no discount: 10 * 6 = 60)
   
   Default service type (uses "standard" when serviceType is omitted):
   ```bash
   curl "http://localhost:5000/factory-demo?price=10&quantity=5"
   ```
   Expected: Same as standard service (applies discount if quantity > 5)
   
   Invalid service type (error):
   ```bash
   curl "http://localhost:5000/factory-demo?price=10&quantity=6&serviceType=invalid"
   ```
   Expected: HTTP 500 error with message about unknown service type