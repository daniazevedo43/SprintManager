using Microsoft.EntityFrameworkCore;
using SprintManager.Application.Interfaces;
using SprintManager.Domain.Entities;
using SprintManager.Infrastructure.Data;

namespace SprintManager.Infrastructure.Repositories
{
    public class WorkItemRepository : IWorkItemRepository
    {
        public readonly ApplicationDbContext _context;

        public WorkItemRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<WorkItem>> GetAllAsync()
        {
            return await _context.WorkItems
                .Include(w => w.Project)
                .Include(w => w.Sprint)
                .Include(w => w.User)
                .OrderBy(w => w.Project)
                .ToListAsync();
        }

        public async Task<List<WorkItem>> GetAllByUserIdAsync(Guid userId)
        {
            return await _context.WorkItems
                .Where(w => w.UserId == userId)
                .ToListAsync();
        }

        public async Task<WorkItem?> GetByIdAsync(Guid id)
        {
            return await _context.WorkItems
                .Include(w => w.Project)
                .Include(w => w.Sprint)
                .Include(w => w.User)
                .FirstOrDefaultAsync(w => w.Id == id);
        }

        public async Task<WorkItem?> GetBySprintIdAsync(Guid sprintId)
        {
            return await _context.WorkItems.FirstOrDefaultAsync(w => w.SprintId == sprintId);
        }

        public async Task AddAsync(WorkItem workItem)
        {
            await _context.WorkItems.AddAsync(workItem);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(WorkItem? workItem)
        {
            if (workItem != null)
            {
                _context.WorkItems.Update(workItem);
            }

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(WorkItem workItem)
        {
            _context.WorkItems.Remove(workItem);
            await _context.SaveChangesAsync();
        }
    }
}