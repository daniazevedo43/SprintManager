using MediatR;
using SprintManager.Application.DTOs;
using SprintManager.Domain.Enums;

namespace SprintManager.Application.Commands.Sprints
{
    public class CreateSprintCommand : IRequest<SprintDTO>
    {
        public Guid ProjectId { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? Description { get; set; }
        public SprintStatus Status { get; set; }
    }
}