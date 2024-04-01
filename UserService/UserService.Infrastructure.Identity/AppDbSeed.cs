using Microsoft.AspNetCore.Identity;
using Common.Enums;

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
    }
}
