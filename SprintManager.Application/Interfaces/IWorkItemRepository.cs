using SprintManager.Domain.Entities;

namespace SprintManager.Application.Interfaces
{
    public interface IWorkItemRepository
    {
        Task<List<WorkItem>> GetAllAsync();
        Task<WorkItem?> GetByIdAsync(Guid id);
        Task AddAsync(WorkItem workItem);
    }
}