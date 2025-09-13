using SprintManager.Domain.Entities;

namespace SprintManager.Application.Interfaces
{
    public interface IRefreshTokenRepository
    {
        Task<RefreshToken?> GetByTokenAsync(string? token);
        Task AddAsync(RefreshToken refreshToken);
    }
}