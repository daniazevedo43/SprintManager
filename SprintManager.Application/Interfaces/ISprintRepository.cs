using SprintManager.Domain.Entities;

namespace SprintManager.Application.Interfaces
{
    public interface ISprintRepository
    {
        Task<List<Sprint>> GetAllAsync();
        Task<Sprint?> GetByIdAsync(Guid id);
        Task<Sprint?> GetByNameAsync(string name);
        Task AddAsync(Sprint sprint);
    }
} 