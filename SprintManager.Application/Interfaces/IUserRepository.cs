using SprintManager.Domain.Entities;

namespace SprintManager.Application.Interfaces
{
    public interface IUserRepository
    {
        Task AddAsync(User user);
        Task<User> GetByEmailAsync(string email);
    }
}
