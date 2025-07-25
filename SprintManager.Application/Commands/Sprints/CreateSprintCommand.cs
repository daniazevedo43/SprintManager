using MediatR;
using SprintManager.Application.DTOs;

namespace SprintManager.Application.Commands.Sprints
{
    public class CreateSprintCommand : IRequest<SprintDTO>
    {
        public Guid ProjectId { get; set; }
        public string SprintName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? Description { get; set; }
    }
}