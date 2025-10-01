using SprintManager.Domain.Entities;

namespace SprintManager.Application.Interfaces
{
    public interface IWorkItemRepository
    {
        Task<List<WorkItem>> GetAllAsync();
        Task<WorkItem?> GetByIdAsync(Guid id);
        Task<List<WorkItem>> GetWorkItemsBySprintIdAsync(Guid sprintId);
        Task<WorkItem?> GetBySprintIdAsync(Guid sprintId);
        Task AddAsync(WorkItem workItem);
        Task UpdateAsync(WorkItem? workItem);
        Task DeleteAsync(WorkItem workItem);
    }
}