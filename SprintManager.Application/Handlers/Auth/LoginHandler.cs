using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using SprintManager.Application.Commands.Auth;
using SprintManager.Application.DTOs;
using SprintManager.Application.Interfaces;
using SprintManager.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;

namespace SprintManager.Application.Handlers.Auth
{
    public class LoginHandler : IRequestHandler<LoginCommand, string>
    {
        private readonly UserManager<User> _userManager;
        private readonly ITokenService _tokenService;

        public LoginHandler(
            UserManager<User> userManager,
            ITokenService tokenService
        )
        {
            _userManager = userManager;
            _tokenService = tokenService;
        }

        public async Task<string> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            var jwtToken = "";

            if (user != null && await _userManager.CheckPasswordAsync(user, request.Password))
            {
                // Call service to create token
                var token = _tokenService.CreateToken(user);
                jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
            }

            return jwtToken;
        }
    }
}