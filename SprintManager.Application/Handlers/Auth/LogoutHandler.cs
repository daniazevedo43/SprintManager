using MediatR;
using Microsoft.AspNetCore.Http;
using SprintManager.Application.Commands.Auth;
using SprintManager.Application.Interfaces;
using System.Security.Claims;

namespace SprintManager.Application.Handlers.Auth
{
    public class LogoutHandler : IRequestHandler<LogoutCommand>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRefreshTokenRepository _refreshTokenRepository;

        public LogoutHandler(
            IHttpContextAccessor httpContextAccessor,
            IRefreshTokenRepository refreshTokenRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _refreshTokenRepository = refreshTokenRepository;
        }

        public async Task Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            var userIdClaim = _httpContextAccessor.HttpContext.User
                .FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId)) 
                throw new UnauthorizedAccessException("User not authenticated.");

            await _refreshTokenRepository.DeleteAllByUserIdAsync(userId);
        }
    }
}