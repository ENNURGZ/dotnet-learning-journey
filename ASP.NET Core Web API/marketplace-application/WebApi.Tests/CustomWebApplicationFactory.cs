using Data.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace WebApi.Tests;

/// <summary>
/// Custom factory for WebApi integration tests with full isolation and in-memory database.
/// </summary>
public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    private static readonly string DatabaseName = $"TestDb_{Guid.NewGuid()}";

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Remove the existing DbContext registration
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<TradeMarketDbContext>));
            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            // Register in-memory DbContext with shared database name for the test instance
            services.AddDbContext<TradeMarketDbContext>(options =>
            {
                options.UseInMemoryDatabase(DatabaseName);
                options.EnableSensitiveDataLogging();
            });
        });

        builder.UseEnvironment("Testing");
    }

    protected override void ConfigureClient(HttpClient client)
    {
        base.ConfigureClient(client);

        // Initialize database once after the application is built
        using var scope = this.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<TradeMarketDbContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<CustomWebApplicationFactory>>();

        try
        {
            db.Database.EnsureCreated();
            UnitTestDataHelper.SeedData(db);
            logger.LogInformation("Test database initialized successfully");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error initializing test database");
            throw;
        }
    }
}
