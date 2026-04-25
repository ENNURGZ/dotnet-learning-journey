# Order Processing: ASP.NET Core Environments

This project **extends** `OrderProcessingWithDi` with ASP.NET Core Environments functionality. It demonstrates working with `IWebHostEnvironment`, environment-specific configuration files, and environment variables.

## Overview

This project is a **continuation** of the `OrderProcessingWithDi` project and includes:
- ✅ All functionality from `OrderProcessingWithDi` (Configuration, DI, Routing, Error Handling)
- ➕ **NEW**: ASP.NET Core Environments functionality

## Prerequisites

**Important:** This project requires the `OrderProcessingWithDi` project as a reference. Make sure you have completed the Configuration assignment.

**Required previous assignments:**
- "Order Processing: Dependency Injection в ASP.NET Core"
- "Order Processing: ASP.NET Core Routing"
- "Order Processing: ASP.NET Core Error Handling"
- **"Order Processing: ASP.NET Core Configuration"** (in `OrderProcessingWithDi` project)

## What's New in This Project

### Additional Endpoint

#### GET `/config/environment` - Get Environment Configuration

Demonstrates access to environment information and environment variables.

**Response:**
```json
{
  "environmentName": "Development",
  "applicationName": "OrderProcessingWithDi",
  "contentRootPath": "/path/to/project",
  "webRootPath": "/path/to/project/wwwroot",
  "pricingFromEnv": "5"
}
```

**What it demonstrates:**
- `IWebHostEnvironment.EnvironmentName` - Current environment (Development, Staging, Production)
- `IWebHostEnvironment.ApplicationName` - Application assembly name
- `IWebHostEnvironment.ContentRootPath` - Application root directory path
- `IWebHostEnvironment.WebRootPath` - Web root directory path
- Configuration values that can be overridden by environment variables

## Running the Project

```bash
# Build all projects (required - this project depends on OrderProcessingWithDi)
dotnet build

# Run the application
dotnet run --project OrderProcessingWithDi

# Or with explicit environment
ASPNETCORE_ENVIRONMENT=Development dotnet run --project OrderProcessingWithDi
```

**Note:** The application runs on `http://localhost:5001` (different port from `OrderProcessingWithDi` which runs on port 5000).

## All Available Endpoints

This project includes **all endpoints** from `OrderProcessingWithDi` plus the new environment endpoint:

### Orders
- `POST /orders` - Create order
- `GET /orders` - Get all orders
- `GET /api/v1/orders/*` - Various order endpoints (see `OrderProcessingWithDi`)

### DI Demo
- `GET /di-demo` - Service lifetimes demonstration
- `GET /factory-demo` - Factory pattern demonstration

### Configuration (from OrderProcessingWithDi)
- `GET /config/pricing` - Get pricing configuration
- `GET /config/order-processing` - Get order processing configuration
- `GET /config/application` - Get application configuration
- `GET /config/all` - Get all configurations
- `GET /config/raw` - Direct IConfiguration access

### Environments (NEW)
- `GET /config/environment` - Get environment information and configuration

## Testing Different Environments

### Development Environment

```bash
export ASPNETCORE_ENVIRONMENT=Development
dotnet run --project OrderProcessingWithDi
curl http://localhost:5001/config/environment
```

Expected: `environmentName` = "Development"

### Production Environment

```bash
export ASPNETCORE_ENVIRONMENT=Production
dotnet run --project OrderProcessingWithDi
curl http://localhost:5001/config/environment
```

Expected: `environmentName` = "Production"

### Environment Variable Override

```bash
export Pricing__DiscountThreshold=10
dotnet run --project OrderProcessingWithDi
curl http://localhost:5001/config/environment
```

Expected: `pricingFromEnv` = "10" (overrides appsettings.json value)

## Configuration Files

### appsettings.json

Base configuration file (same as `OrderProcessingWithDi`):
```json
{
  "Pricing": {
    "DiscountThreshold": 5,
    "DiscountPercentage": 0.1,
    "MinimumOrderValue": 0
  },
  "OrderProcessing": {
    "MaxQuantity": 1000,
    "MaxOrderValue": 10000,
    "EnableValidation": true
  },
  "Application": {
    "ApplicationName": "Order Processing API",
    "Version": "1.0.0",
    "Environment": "Development"
  }
}
```

### appsettings.Development.json

Environment-specific configuration that overrides base values when `ASPNETCORE_ENVIRONMENT=Development`:
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Debug",
      "Microsoft.AspNetCore": "Information"
    }
  }
}
```

## Key Concepts

### IWebHostEnvironment

Service that provides information about the current hosting environment:
- `EnvironmentName` - Current environment name (Development, Staging, Production)
- `ApplicationName` - Application assembly name
- `ContentRootPath` - Application root directory
- `WebRootPath` - Web root directory (wwwroot)

### Environment-Specific Configuration Files

- `appsettings.json` - Base configuration (always loaded)
- `appsettings.{Environment}.json` - Environment-specific overrides
- Loaded automatically based on `ASPNETCORE_ENVIRONMENT` variable

### Environment Variable Overrides

Environment variables can override any configuration value:
- Format: `Section__Property` (double underscore)
- Example: `Pricing__DiscountThreshold=10`
- Maps to: `configuration["Pricing:DiscountThreshold"]`

## Project Structure

```
OrderProcessingWithDi/
├── Program.cs                       # Extends OrderProcessingWithDi with Environments endpoint
├── appsettings.json                 # Base configuration (same as OrderProcessingWithDi)
├── appsettings.Development.json    # Development-specific configuration
├── Properties/
│   └── launchSettings.json         # Launch settings (port 5001)
└── OrderProcessingWithDi.csproj    # Main project file
```

**Note:** This project extends `OrderProcessingWithDi` to reuse all its models, services, and middleware.

## API Testing

Use the included `api.http` file with REST Client extensions:
- **VS Code:** [REST Client](https://marketplace.visualstudio.com/items?itemName=humao.rest-client)
- **JetBrains Rider:** Built-in support
- **Visual Studio:** [REST Client](https://marketplace.visualstudio.com/items?itemName=MadsKristensen.RestClient)

## Differences from OrderProcessingWithDi

| Aspect | OrderProcessingWithDi | OrderProcessingWithDi (with Environments) |
|--------|----------------------|-------------------------------------------|
| **Port** | 5000 | 5001 |
| **Configuration endpoints** | ✅ Yes | ✅ Yes (inherited) |
| **Environments endpoint** | ❌ No | ✅ Yes |
| **All other endpoints** | ✅ Yes | ✅ Yes (inherited) |

## References

- [ASP.NET Core Environments](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/environments)
- [IWebHostEnvironment](https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.hosting.iwebhostenvironment)
- [Environment Variables in Configuration](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/#environment-variables)
