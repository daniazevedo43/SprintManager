using SprintManager.Domain.Enums;

namespace SprintManager.Application.DTOs
{
    public class ProjectMemberBasicDTO
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public ProjectMemberRole Role { get; set; }
    }
}