using SprintManager.Domain.Entities;

namespace SprintManager.Application.Interfaces
{
    public interface IProjectMemberRepository
    {
        Task<ProjectMember?> GetByIdAsync(Guid id);
        Task<ProjectMember?> GetByUserAndProjectIdAsync(Guid userId, Guid projectId);
        Task<List<ProjectMember>> GetMembersByProjectIdAsync(Guid projectId);
        Task AddAsync(ProjectMember projectMember);
        Task DeleteAsync(ProjectMember projectMember);
    }
}