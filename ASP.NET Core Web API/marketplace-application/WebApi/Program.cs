using System.Runtime.InteropServices;
using Business.Interfaces;
using Business.Services;
using Data.Data;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using WebApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Configure basic MVC services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure database context based on operating system
if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
{
    builder.Services.AddDbContext<TradeMarketDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("MarketSqlServer")));
}
else
{
    builder.Services.AddDbContext<TradeMarketDbContext>(options =>
        options.UseSqlite(builder.Configuration.GetConnectionString("Market")));
}

// Register data seeding service
builder.Services.AddScoped<DataSeeder>();

// Register Unit of Work pattern
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Configure AutoMapper for entity-model mapping
builder.Services.AddAutoMapper(cfg => cfg.AddProfile<Business.AutomapperProfile>());

// Register business layer services
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IReceiptService, ReceiptService>();
builder.Services.AddScoped<IStatisticService, StatisticService>();

var app = builder.Build();

// Seed initial data
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var seeder = scope.ServiceProvider.GetRequiredService<DataSeeder>();
    seeder.Seed();
}

// Configure development environment features
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Configure middleware pipeline
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();

// 🚨 CRITICAL: DO NOT DELETE THIS CLASS!
// This partial class is required for integration testing with WebApplicationFactory
// It allows test projects to access the Program class and configure the test host
// Removing this class will break all integration tests in WebApi.Tests project
// This is a .NET 6+ pattern for enabling testing of top-level programs
public partial class Program { }

