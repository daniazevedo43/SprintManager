using Azure.Core;
using Microsoft.EntityFrameworkCore;
using SprintManager.Application.Exceptions;
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

        public async Task<Sprint?> GetByProjectIdAndNameAsync(Guid projectId, string name)
        {
            return await _context.Sprints
                .FirstOrDefaultAsync(s => s.ProjectId == projectId && s.Name == name);
        }

        public async Task AddAsync(Sprint sprint)
        {
            await _context.Sprints.AddAsync(sprint);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Sprint? sprint)
        {
            if (sprint != null)
            {
                var existingSprint = await GetByProjectIdAndNameAsync(sprint.ProjectId, sprint.Name);
                
                if (existingSprint != null) throw new SprintManagerConflictException($"A sprint called '{sprint.Name}' already exists.");

                _context.Sprints.Update(sprint);
            }
            
            await _context.SaveChangesAsync();
        }
    }
}