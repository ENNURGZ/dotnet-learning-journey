# Order Processing: ASP.NET Core Environments

Intermediate level task for practicing ASP.NET Core Environments, working with `IWebHostEnvironment`, and environment-specific configuration.

Estimated time to complete the task: **30 minutes**.

The task requires .NET 8 SDK installed.

## Prerequisites

**Important:** This task requires you to use your completed solution from the previous assignments:
- "Order Processing: Dependency Injection в ASP.NET Core"
- "Order Processing: ASP.NET Core Routing"
- "Order Processing: ASP.NET Core Error Handling"
- "Order Processing: ASP.NET Core Configuration"

## Task Description

Extend the existing `OrderProcessingWithDi` project by implementing environment management using ASP.NET Core Environments system. You will work with `IWebHostEnvironment` to access environment information and demonstrate environment-specific configuration.

All environment functionality should be added without modifying existing endpoint implementations (to maintain backward compatibility with existing tests).

### Project Structure

```
OrderProcessingWithDi/
└── Program.cs                                   # Add environment endpoint
```

## Task Requirements

### 1. Create Environment Demo Endpoint

Add a new endpoint to `Program.cs` that demonstrates access to environment information using `IWebHostEnvironment`.

**Example Minimal API endpoint structure:**
```csharp
app.MapGet("/route", (IConfiguration configuration, IWebHostEnvironment environment) =>
{
    return Results.Ok(new
    {
        property = environment.PropertyName,
        configValue = configuration["Section:Key"]
    });
})
.WithName("EndpointName")
.WithTags("TagName");
```

#### GET `/config/environment` - Get Environment Configuration

Create an endpoint that returns environment information and demonstrates reading configuration that might be set via environment variables.

**Requirements:**
- Route: `/config/environment`
- Inject `IConfiguration` and `IWebHostEnvironment` via method injection
- Return a JSON object with:
  * `environmentName` - from `environment.EnvironmentName`
  * `applicationName` - from `environment.ApplicationName`
  * `contentRootPath` - from `environment.ContentRootPath`
  * `webRootPath` - from `environment.WebRootPath` (can be null)
  * `pricingFromEnv` - from `configuration["Pricing:DiscountThreshold"]`
  * `appSettingsValue` - from `configuration["Application:ApplicationName"]`
- Add `.WithName("GetEnvironmentConfig")` and `.WithTags("Environment")`
- Place this endpoint after all Configuration endpoints but before `app.Run()`

**Hint:** Environment variables can override configuration values. Use double underscore (`__`) for nested keys (e.g., `Pricing__DiscountThreshold` maps to `configuration["Pricing:DiscountThreshold"]`). This was covered in the Configuration topic.

### 2. Understand Environment-Specific Configuration Files

ASP.NET Core automatically loads configuration files based on the environment:
1. First, `appsettings.json` is loaded (base configuration)
2. Then, `appsettings.{Environment}.json` is loaded and overrides matching values from `appsettings.json`

**Note:** The environment is set via `ASPNETCORE_ENVIRONMENT` environment variable (e.g., "Development", "Staging", "Production"). Default: "Production" if not set.

The project already includes `appsettings.Development.json` from the Configuration topic. This file overrides base logging configuration when `ASPNETCORE_ENVIRONMENT=Development`.

**Hint:** You can test different environments by setting the `ASPNETCORE_ENVIRONMENT` environment variable before running the application.

## Validation

1. Run `dotnet build` at the solution root. The project should build without errors.
2. **Important:** Before running tests, make sure you have implemented services from previous assignments (Dependency Injection, Configuration). After implementing services, remove the `Skip` attribute from tests in `OrderProcessingWithDi.Tests/Services/` folder. Tests are currently skipped with `[Fact(Skip = "...")]` until services are implemented.
3. Run `dotnet test` to execute all unit and integration tests. All tests should pass.
4. Start the application with `dotnet run --project OrderProcessingWithDi`.

**Note:** The project includes an `api.http` file in the `OrderProcessingWithDi` folder with pre-configured HTTP requests for testing all endpoints. You can use it with REST Client extensions:
- **VS Code:** [REST Client](https://marketplace.visualstudio.com/items?itemName=humao.rest-client)
- **JetBrains Rider:** Built-in support (no extension needed)
- **Visual Studio:** [REST Client](https://marketplace.visualstudio.com/items?itemName=MadsKristensen.RestClient) or [REST Client File Handler](https://marketplace.visualstudio.com/items?itemName=humao.rest-client)

4. Test the environment endpoint:

   **Get Environment Configuration:**
   ```bash
   curl http://localhost:5000/config/environment
   ```
   Expected response:
   ```json
   {
     "environmentName": "Development",
     "applicationName": "OrderProcessingWithDi",
     "contentRootPath": "/path/to/project/OrderProcessingWithDi",
     "webRootPath": "/path/to/project/OrderProcessingWithDi/wwwroot",
     "pricingFromEnv": "5",
     "appSettingsValue": "Order Processing API"
   }
   ```

   **Note:** Actual paths will vary based on your system and project location. `webRootPath` may be null if wwwroot doesn't exist.

5. Test with different environments:

   **Run in Development mode:**
   ```bash
   export ASPNETCORE_ENVIRONMENT=Development
   dotnet run --project OrderProcessingWithDi
   curl http://localhost:5000/config/environment
   ```
   Expected: `environmentName` should be "Development"

   **Run in Production mode:**
   ```bash
   export ASPNETCORE_ENVIRONMENT=Production
   dotnet run --project OrderProcessingWithDi
   curl http://localhost:5000/config/environment
   ```
   Expected: `environmentName` should be "Production"

6. Test environment variable override (demonstrates interaction with Configuration system):
   ```bash
   export Pricing__DiscountThreshold=10
   dotnet run --project OrderProcessingWithDi
   curl http://localhost:5000/config/environment
   ```
   Expected: `pricingFromEnv` should be "10" (from environment variable, overriding appsettings.json).

## References

- [ASP.NET Core Environments](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/environments)
- [IWebHostEnvironment](https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.hosting.iwebhostenvironment)
- [Environment Variables in Configuration](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/#environment-variables)
