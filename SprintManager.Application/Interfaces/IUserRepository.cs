using SprintManager.Domain.Entities;

namespace SprintManager.Application.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(Guid? id);
    }
}