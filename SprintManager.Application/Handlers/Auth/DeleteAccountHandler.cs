using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using SprintManager.Application.Commands.Auth;
using SprintManager.Application.Interfaces;
using SprintManager.Domain.Entities;
using SprintManager.Exceptions.ExceptionsBase;
using System.Security.Claims;

namespace SprintManager.Application.Handlers.Auth
{
    public class DeleteAccountHandler : IRequestHandler<DeleteAccountCommand>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<User> _userManager;
        private readonly IRefreshTokenRepository _refreshTokenRepository;

        public DeleteAccountHandler(
            IHttpContextAccessor httpContextAccessor,
            UserManager<User> userManager,
            IRefreshTokenRepository refreshTokenRepository
        ) 
        { 
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _refreshTokenRepository = refreshTokenRepository;
        }

        public async Task Handle(DeleteAccountCommand request, CancellationToken cancellationToken)
        {
            var userIdClaim = _httpContextAccessor.HttpContext.User
                .FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
                throw new UnauthorizedAccessException("User not authenticated.");

            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user == null) throw new SprintManagerNotFoundException("User not found.");

            var passwordIsValid = await _userManager.CheckPasswordAsync(user, request.Password);

            if (!passwordIsValid) throw new UnauthorizedAccessException("Invalid password.");

            await _refreshTokenRepository.DeleteAllByUserIdAsync(userId);

            await _userManager.DeleteAsync(user);
        }
    }
}