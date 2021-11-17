using System;
using System.Threading.Tasks;
using ExampleLinkShortener.DataAccess.Entities;
using ExampleLinkShortener.Models;
using Microsoft.AspNetCore.Identity;

namespace ExampleLinkShortener.DataAccess
{
    public class DbSeeder
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DbSeeder(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task Seed()
        {
            var email = "admin@mail.ru";

            foreach (var role in AppRoles.All.Split(','))
            {
                var isNotExist = await _roleManager.FindByNameAsync(role) == null;

                if (isNotExist)
                {
                    await _roleManager.CreateAsync(new IdentityRole(role));
                }
            }
            
            
            var user = await _userManager.FindByEmailAsync(email);
            
            if(user != null)
                return;
            
            
            var adminUser = new User { Email = email, UserName = email, Year = 123 };
            var result = await _userManager.CreateAsync(adminUser, "P@ssw0rd");

            if (result.Succeeded)
            {
                var roleResult = await _userManager.AddToRoleAsync(adminUser, AppRoles.Administrator);
                if (!roleResult.Succeeded)
                {
                    throw new Exception($"Seeder: Add to role {AppRoles.Administrator} failed");
                }
            }
        }
    }
}