using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SprintManager.Application.Interfaces;
using SprintManager.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SprintManager.Infrastructure.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;

        public TokenService(IConfiguration config) 
        {
            _config = config;
        }

        public JwtSecurityToken CreateToken(User user)
        {
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                                 .GetBytes(_config["Jwt:SecretKey"]));
            _ = int.TryParse(_config["Jwt:TokenValidityInMinutes"], out
                int tokenValidityInMinutes);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                expires: DateTime.Now.AddMinutes(tokenValidityInMinutes),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
             );

            return token;
        }
    }
}