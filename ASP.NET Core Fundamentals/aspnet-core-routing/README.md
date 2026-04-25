# Order Processing: ASP.NET Core Routing

Intermediate level task for practicing ASP.NET Core Routing patterns, route constraints, and route groups.

Estimated time to complete the task: **3 hours**.

The task requires .NET 8 SDK installed.

## Prerequisites

**Important:** This task requires you to use your completed solution from the previous assignment "Order Processing: Dependency Injection в ASP.NET Core".

## Task Description

Extend the existing `OrderProcessingWithDi` project by implementing advanced routing patterns. You will add new API endpoints that demonstrate different routing concepts: route parameters, route constraints, route groups, optional parameters, and custom route handlers.

All new endpoints should be added to `Program.cs` without modifying existing endpoint implementations (to maintain backward compatibility with existing tests).

### Project Structure

```
OrderProcessingWithDi/
├── Models/
│   ├── Order.cs                                # Order model (already implemented)
│   └── OrderResult.cs                          # Order processing result (may need to extend)
├── Services/
│   ├── Interfaces/
│   │   └── IOrderRepository.cs                 # TODO: Add GetById method
│   └── Implementations/
│       └── InMemoryOrderRepository.cs          # TODO: Implement GetById method
└── Program.cs                                  # TODO: Add new routing endpoints
```

## Task Requirements

### 1. Extend IOrderRepository Interface

Add the following method to `IOrderRepository` interface:

```csharp
/// <summary>
/// Gets an order result by its ID.
/// </summary>
/// <param name="id">The order ID.</param>
/// <returns>The order result if found, null otherwise.</returns>
OrderResult? GetById(int id);
```

Implement this method in `InMemoryOrderRepository`:
* For this assignment, treat the `id` as a **0-based index** in the in-memory list (index-based approach).
* Implement `GetById` to return `null` when `id < 0` or `id >= orders.Count`.

**Note:** Automated tests expect the **index-based** behavior (0-based) because `OrderResult` does not contain an `Id` field in this project.

### 2. Route Parameters and Constraints

Implement endpoints that use route parameters with constraints:

#### GET `/orders/{orderId:int}` - Get Order by ID

* Use route parameter `{orderId:int}` with constraint to ensure only integers are accepted.
* Handler signature: `(int orderId, IOrderRepository repository) =>`.
* Find order by ID using `repository.GetById(orderId)`.
* Return `Results.Ok(order)` if found, `Results.NotFound()` if not found.
* Add `.WithName("GetOrderByIdLegacy")` and `.WithTags("Orders")` (avoid name conflicts with `/api/v1/orders/{orderId:int}`).

**Important:** Route name must be exactly `"GetOrderById"` for automated testing (the primary `/api/v1/orders/{orderId:int}` endpoint uses this name).

**Example:**
```bash
curl http://localhost:5000/orders/1
curl http://localhost:5000/orders/abc  # Should return 404 (constraint violation)
```

#### GET `/orders/product/{productId:minlength(1)}` - Get Orders by Product ID

* Use route parameter `{productId:minlength(1)}` to ensure non-empty product ID.
* Handler signature: `(string productId, IOrderRepository repository) =>`.
* Filter orders by `productId` using `repository.GetAll()` and LINQ.
* Return `Results.Ok(orders)` (empty list if no orders found).
* Add `.WithName("GetOrdersByProductIdLegacy")` and `.WithTags("Orders")` (avoid name conflicts with `/api/v1/orders/product/{productId:minlength(1)}`).

**Important:** Route name must be exactly `"GetOrdersByProductId"` for automated testing (the primary `/api/v1/orders/product/{productId:minlength(1)}` endpoint uses this name).

**Example:**
```bash
curl http://localhost:5000/orders/product/A1
curl http://localhost:5000/orders/product/  # Should return 404 (constraint violation)
```

#### GET `/orders/range/{minTotal:decimal}/{maxTotal:decimal}` - Get Orders by Total Range

* Use two route parameters with `decimal` constraint.
* Handler signature: `(decimal minTotal, decimal maxTotal, IOrderRepository repository) =>`.
* Filter orders where `total >= minTotal && total <= maxTotal`.
* Return `Results.Ok(orders)`.
* Add `.WithName("GetOrdersByTotalRangeLegacy")` and `.WithTags("Orders")` (avoid name conflicts with `/api/v1/orders/range/{minTotal:decimal}/{maxTotal:decimal}`).

**Important:** Route name must be exactly `"GetOrdersByTotalRange"` for automated testing (the primary `/api/v1/orders/range/{minTotal:decimal}/{maxTotal:decimal}` endpoint uses this name).

**Example:**
```bash
curl http://localhost:5000/orders/range/40/60
```

### 3. Route Groups

Organize related endpoints using route groups:

#### Create Route Group `/api/v1/orders`

Group all order-related endpoints under `/api/v1/orders` prefix:

* Create route group: `var ordersGroup = app.MapGroup("/api/v1/orders")`.
* Add `.WithTags("Orders API v1")` to the group.

**Important:** Route group prefix must be exactly `/api/v1/orders` and tag must be exactly `"Orders API v1"` for automated testing.

* Add new endpoints to the group:
    * `POST /api/v1/orders` - Create Order (duplicate of existing `/orders` endpoint)
    * `GET /api/v1/orders` - Get All Orders (duplicate of existing `/orders` endpoint)
    * `GET /api/v1/orders/{orderId:int}` - Get Order by ID
    * `GET /api/v1/orders/product/{productId:minlength(1)}` - Get Orders by Product ID
    * `GET /api/v1/orders/range/{minTotal:decimal}/{maxTotal:decimal}` - Get Orders by Total Range

**Important for Automated Testing:**
- Keep existing `/orders` endpoints working (they are tested by existing integration tests)
- New endpoints should be added under `/api/v1/orders` route group
- Both old and new endpoints should work simultaneously
- Route names must be unique. If you implement both legacy `/orders/...` endpoints and `/api/v1/orders/...` endpoints, do not reuse the same `.WithName(...)` for both. Use different names for legacy endpoints (e.g., `"GetOrderByIdLegacy"`) or omit `.WithName(...)` on legacy endpoints.

### 4. Optional Parameters and Defaults

#### GET `/api/v1/orders/search` - Search Orders with Optional Parameters

* Use query string parameters (not route parameters) for optional filters.
* Handler signature: `(string? productId, decimal? minTotal, decimal? maxTotal, int? limit, IOrderRepository repository) =>`.
* Apply filters only if parameters are provided:
    * Filter by `productId` if provided.
    * Filter by `minTotal` if provided.
    * Filter by `maxTotal` if provided.
    * Limit results to `limit` items (default: return all).
* Return `Results.Ok(orders)`.
* Add `.WithName("SearchOrders")` and `.WithTags("Orders API v1")`.

**Important:** Route name must be exactly `"SearchOrders"` and tag must be exactly `"Orders API v1"` for automated testing.

**Example:**
```bash
curl "http://localhost:5000/api/v1/orders/search"
curl "http://localhost:5000/api/v1/orders/search?productId=A1"
curl "http://localhost:5000/api/v1/orders/search?minTotal=50&maxTotal=100&limit=5"
```

### 5. Route Constraints - Advanced

#### GET `/api/v1/orders/recent/{days:int:range(1,30)}` - Get Recent Orders

* Use route constraint `{days:int:range(1,30)}` to limit days to 1-30.
* Handler signature: `(int days, IOrderRepository repository) =>`.
* Filter orders created within last `days` days (you may need to add `CreatedAt` to `OrderResult` or use a different approach).
* For simplicity, you can return all orders if adding `CreatedAt` is complex.
* Return `Results.Ok(orders)`.
* Add `.WithName("GetRecentOrders")` and `.WithTags("Orders API v1")`.

**Important:** Route name must be exactly `"GetRecentOrders"` and tag must be exactly `"Orders API v1"` for automated testing.

**Example:**
```bash
curl http://localhost:5000/api/v1/orders/recent/7
curl http://localhost:5000/api/v1/orders/recent/0  # Should return 404 (constraint violation)
curl http://localhost:5000/api/v1/orders/recent/31  # Should return 404 (constraint violation)
```

### 6. Custom Route Handler with Route Values

#### GET `/api/v1/orders/stats` - Get Order Statistics

* Create a route handler that demonstrates accessing route values.
* Handler signature: `(HttpContext context, IOrderRepository repository) =>`.
* Calculate statistics:
    * Total number of orders.
    * Total revenue (sum of all totals).
    * Average order total.
    * Most ordered product ID (product with highest total quantity).
* Return JSON object with statistics.
* Add `.WithName("GetOrderStatistics")` and `.WithTags("Orders API v1")`.

**Important:** Route name must be exactly `"GetOrderStatistics"` and tag must be exactly `"Orders API v1"` for automated testing.

**Example:**
```bash
curl http://localhost:5000/api/v1/orders/stats
```

Expected result:
```json
{
  "totalOrders": 10,
  "totalRevenue": 540.0,
  "averageOrderTotal": 54.0,
  "mostOrderedProductId": "A1"
}
```

### 7. Route Ordering and Precedence

#### GET `/api/v1/orders/{id}` - Catch-all Route (Low Priority)

* Add a catch-all route that handles any string `{id}`.
* This route should have lower precedence than specific routes like `{orderId:int}`.
* Handler signature: `(string id, IOrderRepository repository) =>`.
* Try to parse `id` as integer and find order using `GetById`, or search by product ID.
* Return appropriate result or `Results.NotFound()`.
* Add `.WithName("GetOrderByAnyId")` and `.WithTags("Orders API v1")`.

**Important:** Route name must be exactly `"GetOrderByAnyId"` and tag must be exactly `"Orders API v1"` for automated testing.

**Note:** This route should be registered AFTER more specific routes (like `{orderId:int}`) to demonstrate route precedence.

## Requirements

* Extend `IOrderRepository` interface with `GetById(int id)` method.
* Implement `GetById` in `InMemoryOrderRepository`.
* Use route constraints for parameter validation (`:int`, `:decimal`, `:minlength(n)`, `:range(min,max)`).
* Organize endpoints using route groups (`/api/v1/orders`).
* Use query string parameters for optional filters.
* Ensure proper route ordering (specific routes before catch-all).
* Return appropriate HTTP status codes (200, 404, 400).
* Follow RESTful conventions for endpoint naming.
* Add route names and tags for OpenAPI documentation.
* Keep existing endpoints working (backward compatibility).

## Important Notes for Automated Testing

**This assignment will be automatically tested on autocode.epam.com platform.**

To ensure your solution passes automated tests, please follow these requirements:

1. **Exact Route Names:** Use exact route names as specified (e.g., `"GetOrderById"`, `"GetOrdersByProductId"`).
2. **Exact Tags:** Use exact tags as specified (e.g., `"Orders"`, `"Orders API v1"`).
3. **HTTP Status Codes:** Return correct status codes:
    - `200 OK` for successful operations
    - `404 Not Found` for not found resources and constraint violations
    - `400 Bad Request` for invalid input
4. **Route Group Prefix:** Use exactly `/api/v1/orders` for the route group.
5. **Method Names:** Keep existing method names unchanged (e.g., `ProcessOrderAsync`, `GetAll`).

## Validation

1. Run `dotnet build` at the solution root. The project should build without errors.
2. Run `dotnet test` to execute all unit and integration tests. All tests should pass (including existing DI tests and new routing tests).
3. Start the application with `dotnet run --project OrderProcessingWithDi`.
4. Test the API endpoints:

   **Route Parameters:**
   ```bash
   # Valid integer ID
   curl http://localhost:5000/api/v1/orders/1

   # Invalid ID (should return 404)
   curl http://localhost:5000/api/v1/orders/abc

   # Valid product ID
   curl http://localhost:5000/api/v1/orders/product/A1

   # Invalid product ID (empty, should return 404)
   curl http://localhost:5000/api/v1/orders/product/

   # Valid range
   curl http://localhost:5000/api/v1/orders/range/40/60
   ```

   **Optional Parameters:**
   ```bash
   # No parameters
   curl "http://localhost:5000/api/v1/orders/search"

   # Single parameter
   curl "http://localhost:5000/api/v1/orders/search?productId=A1"

   # Multiple parameters
   curl "http://localhost:5000/api/v1/orders/search?minTotal=50&maxTotal=100&limit=5"
   ```

   **Route Constraints:**
   ```bash
   # Valid range
   curl http://localhost:5000/api/v1/orders/recent/7

   # Invalid (below range)
   curl http://localhost:5000/api/v1/orders/recent/0

   # Invalid (above range)
   curl http://localhost:5000/api/v1/orders/recent/31
   ```

   **Statistics:**
   ```bash
   curl http://localhost:5000/api/v1/orders/stats
   ```

   **Route Groups:**
   ```bash
   # New group endpoint
   curl http://localhost:5000/api/v1/orders

   # Old endpoint (should still work)
   curl http://localhost:5000/orders
   ```

## Expected Response Formats

**Get Order by ID:**
```json
{
  "productId": "A1",
  "quantity": 6,
  "total": 54.0
}
```

**Get Orders (List):**
```json
[
  {
    "productId": "A1",
    "quantity": 6,
    "total": 54.0
  }
]
```

**Order Statistics:**
```json
{
  "totalOrders": 10,
  "totalRevenue": 540.0,
  "averageOrderTotal": 54.0,
  "mostOrderedProductId": "A1"
}
```

**Error Responses:**
- 404 Not Found: No body required
- 400 Bad Request: Error message in response body