using SprintManager.Domain.Enums;

namespace SprintManager.Domain.Entities
{
    public class ProjectMember
    {
        public Guid Id { get; private set; }
        public Guid ProjectId { get; private set; }
        public Guid UserId { get; private set; }
        public ProjectMemberRole Role { get; private set; }

        public ProjectMember()
        {
            
        }

        public ProjectMember(Guid projectId, Guid userId, ProjectMemberRole role)
        {
            if(projectId == Guid.Empty) throw new ArgumentException("Project ID can't be null or empty.", nameof(projectId));
            if(userId == Guid.Empty) throw new ArgumentException("User ID cant be null or empty.", nameof(userId));

            Id = Guid.NewGuid();
            ProjectId = projectId;
            UserId = userId;
            Role = role;
        }

        public void SetProjectId(Guid projectId)
        {
            if(projectId == Guid.Empty) throw new ArgumentException("Project ID can't be null or empty.", nameof(projectId));

            ProjectId = projectId;
        }

        public void SetUserId(Guid userId)
        {
            if(userId == Guid.Empty) throw new ArgumentException("User ID cant be null or empty.", nameof(userId));

            UserId = userId; 
        }

        public void SetRole(ProjectMemberRole role)
        {
            Role = role;
        }
    }
}
