using Microsoft.AspNetCore.Identity;
using UserService.Core.Application.Enums;

namespace UserService.Infrastructure.Identity
{
    internal static class AppDbSeed
    {
        public static void AddRoles(RoleManager<IdentityRole> roleManager)
        {
            IdentityRole[] roles =
            [
                new() { Name = Role.Applicant.ToString() },
                new() { Name = Role.Manager.ToString() },
                new() { Name = Role.MainManager.ToString() },
                new() { Name = Role.Admin.ToString() },
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
    }
}
