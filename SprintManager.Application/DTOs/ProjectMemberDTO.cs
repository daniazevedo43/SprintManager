using SprintManager.Domain.Enums;

namespace SprintManager.Application.DTOs
{
    public class ProjectMemberDto
    {
        public Guid Id { get; set; }
        public Guid ProjectId { get; set; }
        public string ProjectName { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public ProjectMemberRole Role { get; set; }
    }
}