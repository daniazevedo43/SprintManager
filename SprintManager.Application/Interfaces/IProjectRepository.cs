using SprintManager.Domain.Entities;

namespace SprintManager.Application.Interfaces
{
    public interface IProjectRepository
    {
        Task<List<Project>> GetAllAsync();
        Task<Project?> GetByIdAsync(Guid id);
        Task<Project?> GetByNameAsync(string name);
        Task AddAsync(Project project);
        Task UpdateAsync(Project? project);
    }
}