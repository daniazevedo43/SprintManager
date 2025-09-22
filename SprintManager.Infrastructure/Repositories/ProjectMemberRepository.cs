using Microsoft.EntityFrameworkCore;
using SprintManager.Application.Interfaces;
using SprintManager.Domain.Entities;
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

        public async Task<List<ProjectMember>> GetAllAsync()
        {
            return await _context.ProjectMembers
                .Include(pm => pm.Project)
                .Include(pm => pm.User)
                .OrderBy(pm => pm.Project.Name)
                .ThenBy(pm => pm.User.UserName)
                .ToListAsync();
        }

        public async Task<ProjectMember?> GetByIdAsync(Guid id)
        {
            return await _context.ProjectMembers.FindAsync(id);
        }

        public async Task<ProjectMember?> GetByUserAndProjectIdAsync(Guid userId, Guid projectId)
        {
            return await _context.ProjectMembers
                .FirstOrDefaultAsync(pm => pm.UserId == userId && pm.ProjectId == projectId);
        }

        public async Task<List<ProjectMember>> GetMembersByProjectIdAsync(Guid projectId)
        {
            return await _context.ProjectMembers
                .Include(pm => pm.User)
                .Where(pm => pm.ProjectId == projectId)
                .OrderBy(pm => pm.User.UserName)
                .ToListAsync();
        }

        public async Task AddAsync(ProjectMember projectMember)
        {
            await _context.ProjectMembers.AddAsync(projectMember);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ProjectMember? projectMember)
        {
            if (projectMember != null)
            {
                _context.ProjectMembers.Update(projectMember);
            }

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(ProjectMember projectMember)
        {
            _context.ProjectMembers.Remove(projectMember);
            await _context.SaveChangesAsync();
        }
    }
}