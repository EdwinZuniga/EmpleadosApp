using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EmpleadosAPI.Data
{
    public static class AdminUserSeed
    {
        public static async Task SeedAdminUserAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

            var adminEmail = "admin@example.com";
            
            if (userManager.Users.Any(u => u.Email == adminEmail))
                return;
            
            var adminUser = new IdentityUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(adminUser, "Admin123!");
            
            if (!result.Succeeded)
            {
                throw new Exception($"Error al crear el usuario administrador: {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }
        }
    }
}