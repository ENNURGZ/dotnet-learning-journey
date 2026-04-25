# Order Processing: ASP.NET Core Configuration

Intermediate level task for practicing ASP.NET Core Configuration patterns, Options Pattern, and configuration providers.

Estimated time to complete the task: **2 hours**.

The task requires .NET 8 SDK installed.

## Prerequisites

**Important:** This task requires you to use your completed solution from the previous assignments:
- "Order Processing: Dependency Injection в ASP.NET Core"
- "Order Processing: ASP.NET Core Routing"
- "Order Processing: ASP.NET Core Error Handling"

## Task Description

Extend the existing `OrderProcessingWithDi` project by implementing configuration management using ASP.NET Core Configuration system. You will add configuration models, use Options Pattern with `IOptions<T>`, read from appsettings.json, and demonstrate direct `IConfiguration` access.

All configuration should be added without modifying existing endpoint implementations (to maintain backward compatibility with existing tests).

### Project Structure

```
OrderProcessingWithDi/
├── Models/
│   └── Configuration/
│       ├── PricingOptions.cs                    # TODO: Create pricing configuration model
│       ├── OrderProcessingOptions.cs            # TODO: Create order processing configuration model
│       └── ApplicationOptions.cs                # TODO: Create application configuration model
├── Services/
│   └── Implementations/
│       └── PricingService.cs                    # TODO: Update to use IOptions<PricingOptions>
└── Program.cs                                   # TODO: Configure Options Pattern and add configuration endpoints
```

## Task Requirements

### 1. Create Configuration Models

Create three configuration classes in `Models/Configuration/` folder. Each class should have:
- A `SectionName` constant (string) matching the section name in `appsettings.json`
- Properties with default values matching the configuration structure

**Example structure:**
```csharp
public class ExampleOptions
{
    public const string SectionName = "Example";

    public string PropertyName { get; set; } = "default value";
}
```

#### PricingOptions

Create a configuration class for pricing settings with the following properties:
- `DiscountThreshold` (int, default: 5) - quantity threshold for applying discount
- `DiscountPercentage` (decimal, default: 0.1m) - discount percentage (0.1 = 10%)
- `MinimumOrderValue` (decimal, default: 0m) - minimum order value to apply discount

**Requirements:**
- Add `SectionName` constant with value `"Pricing"`
- Provide default values for all properties

#### OrderProcessingOptions

Create a configuration class for order processing settings with the following properties:
- `MaxQuantity` (int, default: 1000) - maximum allowed quantity per order
- `MaxOrderValue` (decimal, default: 10000m) - maximum allowed order value
- `EnableValidation` (bool, default: true) - whether validation is enabled

**Requirements:**
- Add `SectionName` constant with value `"OrderProcessing"`
- Provide default values for all properties

#### ApplicationOptions

Create a configuration class for application settings with the following properties:
- `ApplicationName` (string, default: "Order Processing API")
- `Version` (string, default: "1.0.0")
- `Environment` (string, default: "Development")

**Requirements:**
- Add `SectionName` constant with value `"Application"`
- Provide default values for all properties

**Note:** The `Environment` property here is just a configuration string value, not related to `IWebHostEnvironment.EnvironmentName` (that belongs to ASP.NET Core Environments topic).

### 2. Update appsettings.json

**TODO:** Add configuration sections to `appsettings.json`. The file currently contains only the `Logging` section.

Add three configuration sections after the `Logging` section:

1. **"Pricing"** section with properties matching `PricingOptions` class:
   - `DiscountThreshold` (int): 5
   - `DiscountPercentage` (decimal): 0.1
   - `MinimumOrderValue` (decimal): 0

2. **"OrderProcessing"** section with properties matching `OrderProcessingOptions` class:
   - `MaxQuantity` (int): 1000
   - `MaxOrderValue` (decimal): 10000
   - `EnableValidation` (bool): true

3. **"Application"** section with properties matching `ApplicationOptions` class:
   - `ApplicationName` (string): "Order Processing API"
   - `Version` (string): "1.0.0"
   - `Environment` (string): "Development"

**Requirements:**
- Property names must match configuration class properties (use camelCase in JSON, PascalCase in C#)
- Property types must match (int, decimal, bool, string)
- Don't forget to add commas between sections
- Section names must match the `SectionName` constants from configuration classes

### 3. Register Options in Program.cs

Register all three configuration options using Options Pattern.

**Requirements:**
- Use `builder.Services.Configure<T>()` method
- Use `builder.Configuration.GetSection()` to read from configuration
- Use the `SectionName` constant from each options class
- Register all three options classes: `PricingOptions`, `OrderProcessingOptions`, `ApplicationOptions`

**Hint:** The pattern is: `builder.Services.Configure<TOptions>(builder.Configuration.GetSection(TOptions.SectionName))`

### 4. Update PricingService to Use Configuration

Update `PricingService` to use `IOptions<PricingOptions>` instead of hardcoded constants.

**Required changes:**
- Add constructor parameter: `IOptions<PricingOptions> options`
- Store `options.Value` in a private readonly field
- Replace hardcoded constants with values from the options object
- Update the `CalculateTotal` method to use `this.options.DiscountThreshold`, `this.options.DiscountPercentage`, and `this.options.MinimumOrderValue`

**Hint:** The service currently uses hardcoded constants (`DiscountThreshold = 5`, `DiscountPercentage = 0.1m`, `MinimumOrderValue = 0m`). Inject `IOptions<PricingOptions>` and replace these constants with values from `options.Value`.

### 5. Create Configuration Demo Endpoints

Create endpoints that demonstrate different ways to access configuration.

**Example Minimal API endpoint structure:**
```csharp
app.MapGet("/route", (IOptions<SomeOptions> options) =>
{
    return Results.Ok(options.Value);
})
.WithName("EndpointName")
.WithTags("TagName");
```

#### GET `/config/pricing` - Get Pricing Configuration

Create an endpoint that returns pricing configuration using Options Pattern.

**Requirements:**
- Route: `/config/pricing`
- Inject `IOptions<PricingOptions>` via method injection
- Return `Results.Ok(options.Value)`
- Add `.WithName("GetPricingConfig")` and `.WithTags("Configuration")`

#### GET `/config/order-processing` - Get Order Processing Configuration

Create an endpoint that returns order processing configuration.

**Requirements:**
- Route: `/config/order-processing`
- Inject `IOptions<OrderProcessingOptions>` via method injection
- Return `Results.Ok(options.Value)`
- Add `.WithName("GetOrderProcessingConfig")` and `.WithTags("Configuration")`

#### GET `/config/application` - Get Application Configuration

Create an endpoint that returns application configuration.

**Requirements:**
- Route: `/config/application`
- Inject `IOptions<ApplicationOptions>` via method injection
- Return `Results.Ok(options.Value)`
- Add `.WithName("GetApplicationConfig")` and `.WithTags("Configuration")`

#### GET `/config/all` - Get All Configuration

Create an endpoint that returns all three configurations in a single JSON object.

**Requirements:**
- Route: `/config/all`
- Inject all three `IOptions<T>` types via method injection
- Return a JSON object containing all three configurations (use anonymous object)
- Add `.WithName("GetAllConfig")` and `.WithTags("Configuration")`

**Example:**
```csharp
app.MapGet("/config/all", (IOptions<PricingOptions> pricing, IOptions<OrderProcessingOptions> orderProcessing, IOptions<ApplicationOptions> application) =>
{
    return Results.Ok(new
    {
        pricing = pricing.Value,
        orderProcessing = orderProcessing.Value,
        application = application.Value
    });
})
.WithName("GetAllConfig")
.WithTags("Configuration");
```

### 6. Demonstrate Direct IConfiguration Access

Create an endpoint that demonstrates direct access to `IConfiguration` without using Options Pattern.

#### GET `/config/raw` - Get Raw Configuration

Create an endpoint that demonstrates direct access to `IConfiguration` without using Options Pattern.

**Requirements:**
- Route: `/config/raw`
- Inject `IConfiguration` via method injection
- Use `configuration.GetSection("SectionName").Get<TOptions>()` to read each configuration section
- Return a JSON object with all three configurations
- Add `.WithName("GetRawConfig")` and `.WithTags("Configuration")`

**Hint:** Use `.Get<T>()` method after `GetSection()` to convert configuration section to strongly-typed object.

## Validation

1. Run `dotnet build` at the solution root. The project should build without errors.
2. Run `dotnet test` to execute all unit and integration tests. All tests should pass.
3. Start the application with `dotnet run --project OrderProcessingWithDi`.

**Note:** The project includes an `api.http` file in the `OrderProcessingWithDi` folder with pre-configured HTTP requests for testing all endpoints. You can use it with REST Client extensions:
- **VS Code:** [REST Client](https://marketplace.visualstudio.com/items?itemName=humao.rest-client)
- **JetBrains Rider:** Built-in support (no extension needed)
- **Visual Studio:** [REST Client](https://marketplace.visualstudio.com/items?itemName=MadsKristensen.RestClient) or [REST Client File Handler](https://marketplace.visualstudio.com/items?itemName=humao.rest-client)

4. Test the configuration endpoints:

   **Get Pricing Configuration:**
   ```bash
   curl http://localhost:5000/config/pricing
   ```
   Expected response:
   ```json
   {
     "discountThreshold": 5,
     "discountPercentage": 0.1,
     "minimumOrderValue": 0
   }
   ```

   **Get Order Processing Configuration:**
   ```bash
   curl http://localhost:5000/config/order-processing
   ```
   Expected response:
   ```json
   {
     "maxQuantity": 1000,
     "maxOrderValue": 10000,
     "enableValidation": true
   }
   ```

   **Get Application Configuration:**
   ```bash
   curl http://localhost:5000/config/application
   ```
   Expected response:
   ```json
   {
     "applicationName": "Order Processing API",
     "version": "1.0.0",
     "environment": "Development"
   }
   ```

   **Get All Configuration:**
   ```bash
   curl http://localhost:5000/config/all
   ```
   Expected response:
   ```json
   {
     "pricing": {
       "discountThreshold": 5,
       "discountPercentage": 0.1,
       "minimumOrderValue": 0
     },
     "orderProcessing": {
       "maxQuantity": 1000,
       "maxOrderValue": 10000,
       "enableValidation": true
     },
     "application": {
       "applicationName": "Order Processing API",
       "version": "1.0.0",
       "environment": "Development"
     }
   }
   ```

   **Get Raw Configuration:**
   ```bash
   curl http://localhost:5000/config/raw
   ```
   Expected response: Same format as `/config/all` (all three configurations in one object)

5. Test that PricingService uses configuration:
   ```bash
   curl -X POST "http://localhost:5000/orders?productId=A1&quantity=6&unitPrice=10"
   ```
   Expected: Order should be processed with discount from configuration (discount applied when quantity > 5).

## References

- [ASP.NET Core Configuration](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/)
- [Options Pattern in ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/options)
- [Configuration Providers](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/#configuration-providers)
- [Environment Variables as Configuration Provider](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/#environment-variables)
