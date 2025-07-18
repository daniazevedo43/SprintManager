using SprintManager.Domain.Entities;

namespace SprintManager.Application.Interfaces
{
    public interface IProjectMemberRepository
    {
        Task<ProjectMember?> GetByUserIdAsync(Guid userId, Guid projectId);
        Task AddAsync(ProjectMember projectMember);
    }
}