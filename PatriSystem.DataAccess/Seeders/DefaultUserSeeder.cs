using Microsoft.AspNetCore.Identity;
using PatriSystem.Domain.Entities;

namespace PatriSystem.DataAccess.Seeders
{
    public static class DefaultUserSeeder
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager)
        {
            const string email = "admin@patrisystem.com";
            const string password = "Admin123!";

            var existingUser = await userManager.FindByEmailAsync(email);
            if (existingUser != null) return;

            var user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                FullName = "Administrador",
                EmailConfirmed = true,
                IsActive = true
            };

            await userManager.CreateAsync(user, password);
        }
    }
}