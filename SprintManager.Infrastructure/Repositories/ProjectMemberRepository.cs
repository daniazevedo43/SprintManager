using Microsoft.EntityFrameworkCore;
using SprintManager.Application.Interfaces;
using SprintManager.Domain.Entities;
using SprintManager.Exceptions.ExceptionsBase;
using SprintManager.Infrastructure.Data;

namespace SprintManager.Infrastructure.Repositories
{
    public class ProjectMemberRepository : IProjectMemberRepository
    {
        private readonly ApplicationDbContext _context;

        public ProjectMemberRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ProjectMember?> GetByIdAsync(Guid id)
        {
            return await _context.ProjectMembers.FindAsync(id);
        }

        public async Task<ProjectMember?> GetByUserAndProjectIdAsync(Guid userId, Guid projectId)
        {
            var user = await _context.Users.FindAsync(userId);
            var project = await _context.Projects.FindAsync(projectId);

            if (user == null) throw new SprintManagerNotFoundException($"User with ID {userId} not found");
            if (project == null) throw new SprintManagerNotFoundException($"Project with ID {projectId} was not found");

            return await _context.ProjectMembers
                .FirstOrDefaultAsync(pm => pm.UserId == userId && pm.ProjectId == projectId);
        }

        public async Task<List<ProjectMember>> GetMembersByProjectIdAsync(Guid projectId)
        {
            var project = _context.ProjectMembers
                .FirstOrDefault(pm => pm.ProjectId == projectId);

            if (project == null) throw new SprintManagerNotFoundException($"Project with ID {projectId} was not found");
            
            return await _context.ProjectMembers
                .Include(pm => pm.User)
                .Where(pm => pm.ProjectId == project.ProjectId).ToListAsync();
        }

        public async Task AddAsync(ProjectMember projectMember)
        {
            await _context.ProjectMembers.AddAsync(projectMember);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(ProjectMember projectMember)
        {
            _context.ProjectMembers.Remove(projectMember);
            await _context.SaveChangesAsync();
        }
    }
}