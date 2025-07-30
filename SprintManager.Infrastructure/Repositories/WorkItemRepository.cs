using Microsoft.EntityFrameworkCore;
using SprintManager.Application.Interfaces;
using SprintManager.Domain.Entities;
using SprintManager.Exceptions.ExceptionsBase;
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

        public async Task<WorkItem?> GetByIdAsync(Guid id)
        {
            return await _context.WorkItems
                .Include(w => w.Project)
                .Include(w => w.Sprint)
                .Include(w => w.User)
                .FirstOrDefaultAsync(w => w.Id == id);
        }

        public async Task AddAsync(WorkItem workItem)
        {
            await _context.WorkItems.AddAsync(workItem);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(WorkItem? workItem)
        {
            var sprint = await _context.Sprints.FindAsync(workItem?.SprintId);
            var user = await _context.Users.FindAsync(workItem?.UserId);

            if (workItem != null)
            {
                if (!string.IsNullOrWhiteSpace(workItem.SprintId.ToString()) && sprint == null) throw new SprintManagerNotFoundException($"Sprint with ID {workItem?.SprintId} not found.");
                if (!string.IsNullOrWhiteSpace(workItem.UserId.ToString()) && user == null) throw new SprintManagerNotFoundException($"User with ID {workItem?.UserId} not found.");

                _context.WorkItems.Update(workItem);
            }

            await _context.SaveChangesAsync();
        }
    }
}