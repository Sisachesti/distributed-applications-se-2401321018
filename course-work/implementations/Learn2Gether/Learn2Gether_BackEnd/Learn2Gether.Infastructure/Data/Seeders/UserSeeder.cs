using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Learn2Gether.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Learn2Gether.Infastructure.Data.Seeders
{
    public class UserSeeder
    {
        public async Task SeedUsersAsync(UserManager<User> userManager, RoleManager<IdentityRole<Guid>> roleManager)
        {
            if (userManager == null) throw new ArgumentNullException(nameof(userManager));
            if (roleManager == null) throw new ArgumentNullException(nameof(roleManager));

            string[] roles = new[] { "Student", "Instructor", "Admin" };

            var seedUsers = new[] 
            {
                new{
                    Email = "user1@example.com",
                    FirstName = "John",
                    LastName = "Doe",
                    Password = "Password1",
                    Roles = roles[0]
                },
                new{
                    Email = "user2@example.com",
                    FirstName = "John",
                    LastName = "Doe",
                    Password = "Password2",
                    Roles = roles[0]
                },
                new{
                    Email = "user3@example.com",
                    FirstName = "John",
                    LastName = "Doe",
                    Password = "Password3",
                    Roles = roles[1]
                },
                new
                {
                    Email = "admin@example.com",
                    FirstName = "Admin",
                    LastName = "User",
                    Password = "AdminPassword1",
                    Roles = roles[2]
                }
            };

            foreach (var user in seedUsers)
            {
                var existing = await userManager.FindByEmailAsync(user.Email);
                if (existing == null)
                {
                    var newUser = new User
                    {
                        UserName = user.Email,
                        Email = user.Email,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        EmailConfirmed = true
                    };

                    var createResult = await userManager.CreateAsync(newUser, user.Password);
                    if (!createResult.Succeeded)
                    {
                        continue;
                    }

                    var addRolesResult = await userManager.AddToRolesAsync(newUser, new[] { user.Roles });
                    if (!addRolesResult.Succeeded)
                    {
                        continue;
                    }
                }
            }
        }
    }
}
