using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace SportsStore.Models;

public static class IdentitySeedData
{
    private const string adminUser = "Admin";
    private const string adminPassword = "Secret123$";

    public static async Task EnsurePopulated(IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        AppIdentityDbContext context = scope.ServiceProvider
            .GetRequiredService<AppIdentityDbContext>();

        if (context.Database.ProviderName == "Microsoft.EntityFrameworkCore.Sqlite")
        {
            await context.Database.EnsureCreatedAsync();
        }
        else if (context.Database.ProviderName != "Microsoft.EntityFrameworkCore.InMemory" &&
            (await context.Database.GetPendingMigrationsAsync()).Any())
        {
            await context.Database.MigrateAsync();
        }

        UserManager<IdentityUser> userManager = scope.ServiceProvider
            .GetRequiredService<UserManager<IdentityUser>>();

        IdentityUser? user = await userManager.FindByNameAsync(adminUser);

        if (user is null)
        {
            user = new IdentityUser(adminUser)
            {
                Email = "admin@example.com",
                PhoneNumber = "555-1234"
            };
            await userManager.CreateAsync(user, adminPassword);
        }
    }
}
