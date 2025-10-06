using MediatR;
using SprintManager.Application.DTOs;
using SprintManager.Domain.Enums;

namespace SprintManager.Application.Commands.Sprints
{
    public class UpdateSprintCommand : IRequest<SprintDto>
    {
        public Guid Id { get; set; }
        public string SprintName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? Description { get; set; }
        public SprintStatus Status { get; set; }
    }
}