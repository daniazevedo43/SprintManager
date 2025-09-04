using MediatR;
using Microsoft.AspNetCore.Identity;
using SprintManager.Application.Commands.Auth;
using SprintManager.Application.Interfaces;
using SprintManager.Domain.Entities;
using SprintManager.Exceptions.ExceptionsBase;
using System.IdentityModel.Tokens.Jwt;

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

            if (user == null) throw new SprintManagerNotFoundException("Invalid email or password.");

            if (!await _userManager.IsEmailConfirmedAsync(user))
            {
                throw new Exception("Email not confirmed. Please check your inbox");
            }

            var passwordExists = await _userManager.CheckPasswordAsync(user, request.Password);

            if (!passwordExists) throw new SprintManagerNotFoundException("Invalid password.");

            // Call service to create token
            var token = _tokenService.CreateToken(user);
            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

            return jwtToken;
        }
    }
}