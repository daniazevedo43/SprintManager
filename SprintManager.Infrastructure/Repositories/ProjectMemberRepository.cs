using Azure.Core;
using MediatR;
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

        public async Task<ProjectMember?> GetByUserIdAsync(Guid userId, Guid projectId)
        {
            var user = await _context.Users.FindAsync(userId);
            var project = await _context.Projects.FindAsync(projectId);

            if (user == null) throw new SprintManagerNotFoundException($"User with ID {userId} not found");
            if (project == null) throw new SprintManagerNotFoundException($"Project with ID {projectId} was not found");

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