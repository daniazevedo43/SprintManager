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

        public async Task<ProjectMember?> GetByUserIdAsync(Guid userId, Guid projectId)
        {
            return await _context.ProjectMembers
                .FirstOrDefaultAsync(pm => pm.UserId == userId && pm.ProjectId == projectId);
        }

        public async Task AddAsync(ProjectMember projectMember)
        {
            await _context.ProjectMembers.AddAsync(projectMember);
            await _context.SaveChangesAsync();
        }
    }
}