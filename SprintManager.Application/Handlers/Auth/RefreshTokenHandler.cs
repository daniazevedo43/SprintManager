using MediatR;
using Microsoft.AspNetCore.Identity;
using SprintManager.Application.Commands.Auth;
using SprintManager.Application.DTOs;
using SprintManager.Application.Interfaces;
using SprintManager.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;

namespace SprintManager.Application.Handlers.Auth
{
    public class RefreshTokenHandler : IRequestHandler<RefreshTokenCommand, LoginDTO>
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly UserManager<User> _userManager;
        private readonly ITokenService _tokenService;

        public RefreshTokenHandler(
            IRefreshTokenRepository refreshTokenRepository, 
            UserManager<User> userManager, 
            ITokenService tokenService
        )
        {
            _refreshTokenRepository = refreshTokenRepository;
            _userManager = userManager;
            _tokenService = tokenService;
        }

        public async Task<LoginDTO> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var refreshToken = await _refreshTokenRepository.GetByTokenAsync(request.RefreshToken);
       
            if (refreshToken == null || refreshToken.IsRevoked || refreshToken.ExpirationDate < DateTime.UtcNow)
            {
                throw new UnauthorizedAccessException("Invalid or expired refresh token.");
            }

            var user = await _userManager.FindByIdAsync(refreshToken.UserId.ToString());
            
            if (user == null)
            {
                throw new UnauthorizedAccessException("User not found");
            }

            refreshToken.IsRevoked = true;
            await _refreshTokenRepository.UpdateAsync(refreshToken);

            var token = _tokenService.CreateToken(user);
            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

            var newRefreshToken = new RefreshToken
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                Token = _tokenService.GenerateRefreshToken(),
                ExpirationDate = DateTime.UtcNow.AddDays(7),
                IsRevoked = false
            };

            await _refreshTokenRepository.AddAsync(newRefreshToken);

            return new LoginDTO
            {
                AccessToken = jwtToken,
                RefreshToken = newRefreshToken.Token
            };
        }
    }
}