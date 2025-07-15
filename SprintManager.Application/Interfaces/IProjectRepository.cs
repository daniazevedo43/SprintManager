using SprintManager.Domain.Entities;

namespace SprintManager.Application.Interfaces
{
    public interface IProjectRepository
    {
        Task<List<Project>> GetAllAsync();
    }
}
