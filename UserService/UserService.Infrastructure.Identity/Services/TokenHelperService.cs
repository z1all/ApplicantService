using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using UserService.Core.Domain.Entities;
using Common.API.Configurations.Authorization;

namespace UserService.Infrastructure.Identity.Services
{
    internal class TokenHelperService(IOptions<JwtOptions> options, UserManager<CustomUser> userManager)
    {
        private readonly JwtOptions jwtOptions = options.Value;
        private readonly JwtSecurityTokenHandler _tokenHandler = new();
        private readonly UserManager<CustomUser> _userManager = userManager;

        public async Task<(string token, Guid JTI)> GenerateJWTTokenAsync(CustomUser user)
        {
            byte[] key = Encoding.ASCII.GetBytes(jwtOptions.SecretKey);
            (List <Claim> claims, Guid JTI) = await GetClaims(user);

            var tokenDescription = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(jwtOptions.AccessTokenTimeLifeMinutes),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key), 
                    SecurityAlgorithms.HmacSha256Signature
                ),
            };

            var token = _tokenHandler.CreateToken(tokenDescription);
            return (_tokenHandler.WriteToken(token), JTI);
        }

        private async Task<(List<Claim> claims, Guid JTI)> GetClaims(CustomUser user)
        {
            Guid JTI = Guid.NewGuid();

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, JTI.ToString()),
            };

            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            return (claims, JTI);
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];

            using var generator = RandomNumberGenerator.Create();

            generator.GetBytes(randomNumber);

            return Convert.ToBase64String(randomNumber);
        }
    }
}
