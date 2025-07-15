using SprintManager.Domain.Enums;

namespace SprintManager.Application.DTOs
{
    public class ProjectDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public DateTime CreationDate { get; set; }
        public ProjectStatus Status { get; set; }
    }
}
