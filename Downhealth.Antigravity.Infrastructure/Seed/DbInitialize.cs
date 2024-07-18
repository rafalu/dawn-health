using Dawnhealth.Antigravity.Domain.Users;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Downhealth.Antigravity.Infrastructure.Seed;

public static class DbInitialize
{
    public static async Task Initialize(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        await dbContext.Database.EnsureCreatedAsync();
        await dbContext.Database.MigrateAsync();

        var admin = new ApplicationUser
        {
            UserName = "admin@antigravity.com",
            Email = "admin@antigravity.com",
            EmailConfirmed = true,
            FirstName = "Admin",
            LastName = "Admin",
        };
        string[] adminRoleNames = { "Administrator" };

        //Ensure this comes from a secure source
        var password = "Asdfasdf1";

        await CreateUserWithRoles(admin, password, adminRoleNames, serviceProvider);
    }

    private async static Task CreateUserWithRoles(ApplicationUser user, string password, string[] roleNames, IServiceProvider serviceProvider)
    {
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

        var exitingUser = await userManager.FindByEmailAsync(user.Email);
        if (exitingUser == null)
        {
            //add new user
            var result = await userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                foreach (var roleName in roleNames)
                {
                    await userManager.AddToRoleAsync(user, roleName);
                }
            }
        }
        else
        {
            //update user
            foreach (var roleName in roleNames)
            {
                if (!await userManager.IsInRoleAsync(exitingUser, roleName))
                    await userManager.AddToRoleAsync(exitingUser, roleName);
            }
        }
    }
}
