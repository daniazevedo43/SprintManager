using SprintManager.Domain.Enums;

namespace SprintManager.Application.DTOs
{
    public class ProjectMemberDTO
    {
        public Guid Id { get; set; }
        public Guid ProjectId { get; set; }
        public Guid UserId { get; set; }
        public ProjectMemberRole Role { get; set; }
    }
}