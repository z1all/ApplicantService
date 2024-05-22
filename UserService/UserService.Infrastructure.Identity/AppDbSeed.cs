using Microsoft.AspNetCore.Identity;
using UserService.Core.Application.Interfaces;
using UserService.Core.Application.DTOs;
using Common.Models.Enums;
using UserService.Core.Domain.Entities;

namespace UserService.Infrastructure.Identity
{
    internal static class AppDbSeed
    {
        public static void AddRoles(RoleManager<IdentityRole> roleManager)
        {
            IdentityRole[] roles =
            [
                new() { Name = Role.Applicant },
                new() { Name = Role.Manager },
                new() { Name = Role.MainManager },
                new() { Name = Role.Admin },
            ];

            List<IdentityRole> existRoles = roleManager.Roles.ToList();
            foreach (var role in roles)
            {
                if(!existRoles.Any(existRole => existRole.Name == role.Name))
                {
                     roleManager.CreateAsync(role).Wait();
                }
            }
        }

        public static void AddAdmins(IProfileService profileService, UserManager<CustomUser> userManager)
        {
            try
            {
                List<CreateAdminRequestDTO> admins = [
                    new()
                    {
                        Email = "admin1@gmail.com",
                        FullName = "admin1",
                        Password = "stringA1",
                    },
                    new()
                    {
                        Email = "admin2@gmail.com",
                        FullName = "admin2",
                        Password = "stringA1",
                    },
                    new()
                    {
                        Email = "admin3@gmail.com",
                        FullName = "admin3",
                        Password = "stringA1",
                    }
                ];

                foreach (var admin in admins)
                {
                    CustomUser? customUser = userManager.FindByEmailAsync(admin.Email).GetAwaiter().GetResult();
                    if (customUser is null)
                    {
                        profileService.CreateAdminAsync(admin).Wait();
                    }  
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
