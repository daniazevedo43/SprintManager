using Microsoft.EntityFrameworkCore;
using SprintManager.Application.Interfaces;
using SprintManager.Domain.Entities;
using SprintManager.Infrastructure.Data;

namespace SprintManager.Infrastructure.Repositories
{
    public class SprintRepository : ISprintRepository
    {
        private readonly ApplicationDbContext _context;

        public SprintRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Sprint>> GetAllAsync()
        {
            return await _context.Sprints.OrderBy(s => s.Name).ToListAsync();
        }

        public async Task<Sprint?> GetByIdAsync(Guid id)
        {
            return await _context.Sprints.FindAsync(id);
        }

        public async Task<Sprint?> GetByNameAsync(string name)
        {
            return await _context.Sprints.FirstOrDefaultAsync(s => s.Name == name);
        }

        public async Task AddAsync(Sprint sprint)
        {
            await _context.Sprints.AddAsync(sprint);
            await _context.SaveChangesAsync();
        }
    }
}