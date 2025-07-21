using SprintManager.Domain.Enums;

namespace SprintManager.Domain.Entities
{
    public class ProjectMember
    {
        public Guid Id { get; private set; }
        public Guid ProjectId { get; private set; }
        public Guid UserId { get; private set; }
        public ProjectMemberRole Role { get; private set; }
        public Project? Project { get; private set; }
        public User? User { get; private set; }

        public ProjectMember()
        {
            
        }

        public ProjectMember(Guid projectId, Guid userId, ProjectMemberRole role)
        {
            if (projectId == Guid.Empty) throw new ArgumentNullException(nameof(projectId), "Project ID can't be null or empty.");
            if (userId == Guid.Empty) throw new ArgumentNullException(nameof(userId), "User ID can't be null or empty.");

            Id = Guid.NewGuid();
            ProjectId = projectId;
            UserId = userId;
            Role = role;
        }

        public void SetRole(ProjectMemberRole role)
        {
            Role = role;
        }
    }
}