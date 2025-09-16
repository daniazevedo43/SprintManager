using SprintManager.Domain.Entities;

namespace SprintManager.Application.Interfaces
{
    public interface IUserRepository
    {
        Task<List<User>> GetAllAsync();
        Task<User?> GetByIdAsync(Guid? id);
    }
}