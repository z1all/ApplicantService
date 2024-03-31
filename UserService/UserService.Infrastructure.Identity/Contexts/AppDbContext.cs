using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UserService.Core.Domain.Entities;

namespace UserService.Infrastructure.Identity.Contexts
{
    internal class AppDbContext : IdentityDbContext<CustomUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> option) : base(option) {}
    }
}
