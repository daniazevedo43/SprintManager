using SprintManager.Domain.Entities;

namespace SprintManager.Application.Interfaces
{
    public interface ISprintRepository
    {
        Task<List<Sprint>> GetAllAsync();
    }
}