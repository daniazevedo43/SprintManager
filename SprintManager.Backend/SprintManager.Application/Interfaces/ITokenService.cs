using SprintManager.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;

namespace SprintManager.Application.Interfaces
{
    public interface ITokenService
    {
        JwtSecurityToken CreateToken(User user);
        string GenerateRefreshToken();
    }
}