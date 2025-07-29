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

        public async Task<WorkItem?> GetByIdAsync(Guid id)
        {
            return await _context.WorkItems.FindAsync(id);
        }
    }
}