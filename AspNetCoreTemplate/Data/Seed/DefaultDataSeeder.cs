using AspNetCoreTemplate.Data.Models;
using Microsoft.AspNetCore.Identity;

namespace AspNetCoreTemplate.Data.Seed
{
    public class DefaultDataSeeder
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;

        public DefaultDataSeeder(IServiceProvider services)
        {
            _userManager = services.GetRequiredService<UserManager<AppUser>>();
            _roleManager = services.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
        }

        public DefaultDataSeeder(UserManager<AppUser> userManager, RoleManager<IdentityRole<Guid>> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task SeedAsync()
        {
            // Seed Default Roles
            var defaultRoles = new[] { "Admin" };
            foreach (var roleName in defaultRoles)
            {
                var roleExists = await _roleManager.RoleExistsAsync(roleName);
                if (!roleExists)
                {
                    var role = new IdentityRole<Guid>(roleName);
                    await _roleManager.CreateAsync(role);
                }
            }

            // Seed Default User
            var defaultUser = new AppUser { UserName = "SomeUser", Email = "gbdrm1@gmail.com" };
            var userExists = await _userManager.FindByEmailAsync(defaultUser.Email);
            if (userExists == null)
            {
                var createResult = await _userManager.CreateAsync(defaultUser, "SomePassword");
                if (createResult.Succeeded)
                {
                    await _userManager.AddToRoleAsync(defaultUser, "Admin");
                }
            }
        }
    }

}
