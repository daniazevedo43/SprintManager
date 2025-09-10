using SprintManager.Domain.Entities;

namespace SprintManager.Application.Interfaces
{
    public interface IRefreshTokenRepository
    {
        Task AddAsync(RefreshToken refreshToken);
    }
}