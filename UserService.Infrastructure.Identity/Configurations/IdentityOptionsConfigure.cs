using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace UserService.Infrastructure.Identity.Configurations
{
    internal class IdentityOptionsConfigure : IConfigureOptions<IdentityOptions>
    {
        public void Configure(IdentityOptions options)
        {
            options.Password.RequireDigit = true; // Требование использования цифр
            options.Password.RequireLowercase = true; // Требование использования символов в нижнем регистре
            options.Password.RequireUppercase = true; // Требование использования символов в верхнем регистре
            options.Password.RequireNonAlphanumeric = false; // Требование использования специальных символов
            options.Password.RequiredLength = 6; // Минимальная длина пароля
            options.Password.RequiredUniqueChars = 0; // Минимальное количество уникальных символов
        }
    }
}
