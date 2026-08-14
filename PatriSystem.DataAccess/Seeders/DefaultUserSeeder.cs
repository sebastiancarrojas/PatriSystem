using Microsoft.AspNetCore.Identity;
using PatriSystem.Domain.Entities;

namespace PatriSystem.DataAccess.Seeders
{
    public static class DefaultUserSeeder
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager)
        {
            await SeedUserAsync(userManager, "admin@patrisystem.com", "Admin123!", "Administrador");
            await SeedUserAsync(userManager, "sebascarrojas@gmail.com", "Admin123!", "Sebastian Carrojas");
        }

        private static async Task SeedUserAsync(
            UserManager<ApplicationUser> userManager,
            string email,
            string password,
            string fullName)
        {
            var existingUser = await userManager.FindByEmailAsync(email);
            if (existingUser != null) return;

            var user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                FullName = fullName,
                EmailConfirmed = true,
                IsActive = true
            };

            await userManager.CreateAsync(user, password);
        }
    }
}