using MediatR;
using Microsoft.AspNetCore.Identity;
using SprintManager.Application.Commands.Auth;
using SprintManager.Application.DTOs;
using SprintManager.Application.Interfaces;
using SprintManager.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;

namespace SprintManager.Application.Handlers.Auth
{
    public class RefreshHandler : IRequestHandler<RefreshCommand, LoginDTO>
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly ITokenService _tokenService;
        private readonly UserManager<User> _userManager;

        public RefreshHandler(
            IRefreshTokenRepository refreshTokenRepository, 
            ITokenService tokenService,
            UserManager<User> userManager
        )
        {
            _refreshTokenRepository = refreshTokenRepository;
            _tokenService = tokenService;
            _userManager = userManager;
        }

        public async Task<LoginDTO> Handle(RefreshCommand request, CancellationToken cancellationToken)
        {
            var refreshToken = await _refreshTokenRepository.GetByTokenAsync(request.RefreshToken);
       
            if (refreshToken == null || refreshToken.ExpirationDate < DateTime.UtcNow)
            {
                throw new UnauthorizedAccessException("Invalid or expired refresh token.");
            }

            var user = await _userManager.FindByIdAsync(refreshToken.UserId.ToString());
            
            if (user == null)
            {
                throw new UnauthorizedAccessException("User not found.");
            }

            await _refreshTokenRepository.DeleteAsync(refreshToken);

            var token = _tokenService.CreateToken(user);
            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

            var newRefreshToken = new RefreshToken(user.Id, _tokenService.GenerateRefreshToken());

            await _refreshTokenRepository.AddAsync(newRefreshToken);

            return new LoginDTO
            {
                AccessToken = jwtToken,
                RefreshToken = newRefreshToken.Token
            };
        }
    }
}