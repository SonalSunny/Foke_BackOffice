using FOKE.Services.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FOKE.Services.Repository
{
    public class AuthenticationServiceRepository : IAuthenticationServiceRepository
    {
        private readonly IConfiguration _config;
        public AuthenticationServiceRepository(IConfiguration config)
        {
            _config = config;
        }

        public async Task<string> GenerateToken(string devicePrimaryId, string deviceId, string civilId)
        {
            var claims = new[]
            {
                new Claim("deviceId", deviceId),
                new Claim("civilId", civilId),
                new Claim("devicePrimaryId", devicePrimaryId)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(_config["Jwt:ExpireMinutes"])),
            signingCredentials: creds);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
