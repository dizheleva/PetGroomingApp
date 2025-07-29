namespace PetGroomingApp.Data.Seeding
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.DependencyInjection;

    public class RoleSeedier
    {
        public static async Task SeedRolesAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

            string[] roles = { "Admin", "Manager", "Groomer", "User" };
            
            foreach (string role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            if (roleManager == null)
            {
                var user = new IdentityUser
                {
                    UserName = "mangerEmail",
                    Email = "managerEmail",
                };

                var result = await userManager.CreateAsync(user, "123456");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Manager");
                }
            }
        }
    }
}
