using SprintManager.Domain.Entities;

namespace SprintManager.Application.Interfaces
{
    public interface IWorkItemRepository
    {
        Task<List<WorkItem>> GetAllAsync();
        Task<List<WorkItem>> GetAllByUserIdAsync(Guid userId);
        Task<WorkItem?> GetByIdAsync(Guid id);
        Task<WorkItem?> GetBySprintIdAsync(Guid sprintId);
        Task AddAsync(WorkItem workItem);
        Task UpdateAsync(WorkItem? workItem);
        Task DeleteAsync(WorkItem workItem);
    }
}