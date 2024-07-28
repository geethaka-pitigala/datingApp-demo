using API.Entities;
using API.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API.Services;

public class TokenService(IConfiguration config) : ITokenService
{
        public string GenerateJSONWebToken(AppUser user)
        {   
            var key = config["Jwt:Key"] ?? throw new Exception("Jwt key not found in app settings file");
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserName),
            };

            var token = new JwtSecurityToken(config["Jwt:Issuer"],
              config["Jwt:Issuer"],
              claims,
              expires: DateTime.Now.AddMinutes(120),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
}
