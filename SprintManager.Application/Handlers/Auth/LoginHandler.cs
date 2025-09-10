using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using SprintManager.Application.Commands.Auth;
using SprintManager.Application.DTOs;
using SprintManager.Application.Interfaces;
using SprintManager.Domain.Entities;
using SprintManager.Exceptions.ExceptionsBase;
using System.IdentityModel.Tokens.Jwt;

namespace SprintManager.Application.Handlers.Auth
{
    public class LoginHandler : IRequestHandler<LoginCommand, LoginDTO>
    {
        private readonly UserManager<User> _userManager;
        private readonly ITokenService _tokenService;
        private readonly IRefreshTokenRepository _refreshTokenRepository;

        public LoginHandler(
            UserManager<User> userManager,
            ITokenService tokenService,
            IRefreshTokenRepository refreshTokenRepository
        )
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _refreshTokenRepository = refreshTokenRepository;
        }

        public async Task<LoginDTO> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null) throw new SprintManagerNotFoundException("Invalid email or password.");

            if (!await _userManager.IsEmailConfirmedAsync(user))
            {
                throw new SprintManagerEmailNotConfirmed("Email not confirmed.");
            }

            var passwordExists = await _userManager.CheckPasswordAsync(user, request.Password);

            if (!passwordExists) throw new SprintManagerNotFoundException("Invalid password.");

            // Call service to create token
            var token = _tokenService.CreateToken(user);
            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

            var refreshToken = new RefreshToken
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                Token = Guid.NewGuid().ToString(),
                Expires = DateTime.UtcNow.AddDays(7),
                IsRevoked = false
            };

            await _refreshTokenRepository.AddAsync(refreshToken);

            return new LoginDTO
            {
                AccessToken = jwtToken,
                RefreshToken = refreshToken.Token
            };
        }
    }
}