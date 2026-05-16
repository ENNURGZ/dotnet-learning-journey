using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SportsStore.Models;
using SportsStore.Models.Repository;

var builder = WebApplication.CreateBuilder(args);

// NEW: Add MVC services
builder.Services.AddControllersWithViews();

// NEW: Add DbContext service with cross-platform support
builder.Services.AddDbContext<StoreDbContext>(opts => {
    var connectionString = builder.Configuration["ConnectionStrings:SportsStoreConnection"];

    if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX) || RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
    {
        opts.UseSqlite(builder.Configuration["ConnectionStrings:SqliteConnection"] ?? "Data Source=sportsstore.db");
    }
    else
    {
        // Use SQL Server on Windows
        opts.UseSqlServer(connectionString ?? "Server=(localdb)\\MSSQLLocalDB;Database=SportsStore;MultipleActiveResultSets=true");
    }
});

builder.Services.AddDbContext<AppIdentityDbContext>(opts =>
{
    if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX) || RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
    {
        opts.UseSqlite("Data Source=identity.db");
    }
    else
    {
        opts.UseSqlServer(builder.Configuration["ConnectionStrings:IdentityConnection"]);
    }
});

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<AppIdentityDbContext>();

// NEW: Add repository service
builder.Services.AddScoped<IStoreRepository, EfStoreRepository>();
builder.Services.AddScoped<IOrderRepository, EfOrderRepository>();

// NEW: Add distributed memory cache and session support
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

// NEW: Register Cart service and HttpContextAccessor for DI
builder.Services.AddScoped<Cart>(SessionCart.GetCart);
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

var app = builder.Build();

if (app.Environment.IsProduction())
{
    app.UseExceptionHandler("/Error");
}

app.UseStatusCodePages();
app.UseStaticFiles();

// NEW: Explicitly add routing and session
app.UseRouting();
app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

// NEW: Custom routes for category and pagination
app.MapControllerRoute(
    name: "pagination",
    pattern: "Products/Page{productPage:int}",
    defaults: new { Controller = "Home", action = "Index", productPage = 1 });

app.MapControllerRoute(
     name: "categoryPage",
     pattern: "{category}/Page{productPage:int}",
     defaults: new { Controller = "Home", action = "Index" });
  
app.MapControllerRoute(
    name: "category",
    pattern: "Products/{category}",
    defaults: new { Controller = "Home", action = "Index", productPage = 1 });

app.MapControllerRoute(
    name: "shoppingCart",
    pattern: "Cart",
    defaults: new { Controller = "Cart", action = "Index" });

// NEW: Add remove and checkout routes
app.MapControllerRoute(
    name: "remove",
    pattern: "Remove",
    defaults: new { Controller = "Cart", action = "Remove" });

app.MapControllerRoute(
    name: "checkout",
    pattern: "Checkout",
    defaults: new { Controller = "Order", action = "Checkout" });

app.MapControllerRoute(
    name: "default",
    pattern: "/",
    defaults: new { Controller = "Home", action = "Index" });

app.MapControllerRoute(
    name: "error",
    pattern: "Error",
    defaults: new { Controller = "Home", action = "Error" });

app.MapDefaultControllerRoute();

// NEW: Initialize database with seed data
SeedData.EnsurePopulated(app);
await IdentitySeedData.EnsurePopulated(app);

await app.RunAsync();

public partial class Program 
{ 
    protected Program() { }
}