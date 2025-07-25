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
            return await _context.Sprints
                .Include(s => s.Project)
                .OrderBy(s => s.Project)
                .ToListAsync();
        }
        //COMMIT TEST
        public async Task<Sprint?> GetByIdAsync(Guid id)
        {
            return await _context.Sprints
                .Include(s => s.Project)
                .Select(s => s)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<Sprint?> GetByProjectIdAndSprintNameAsync(Guid projectId, string sprintName)
        {
            return await _context.Sprints
                .FirstOrDefaultAsync(s => s.ProjectId == projectId && s.SprintName == sprintName);
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
                var existingSprint = await GetByProjectIdAndSprintNameAsync(sprint.ProjectId, sprint.SprintName);

                var remainingSprints = await _context.Sprints
                    .Where(s => s.Id != sprint.Id &&
                                s.ProjectId == sprint.ProjectId &&
                                s.SprintName == sprint.SprintName)
                    .Select(s => s)
                    .ToListAsync();


                if (remainingSprints.Count > 0) throw new SprintManagerConflictException($"A sprint called '{sprint.SprintName}' already exists.");

                _context.Sprints.Update(sprint);
            }
            
            await _context.SaveChangesAsync();
        }
    }
}