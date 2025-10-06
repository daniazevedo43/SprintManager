using SprintManager.Domain.Enums;

namespace SprintManager.Application.DTOs
{
    public class SprintDto
    {
        public Guid Id { get; set; }
        public Guid ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string SprintName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? Description { get; set; }
        public SprintStatus Status { get; set; }
    }
}
