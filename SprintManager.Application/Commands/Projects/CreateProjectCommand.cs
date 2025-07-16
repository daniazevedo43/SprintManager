using MediatR;
using SprintManager.Application.DTOs;
using SprintManager.Domain.Enums;

namespace SprintManager.Application.Commands.Projects
{
    public class CreateProjectCommand : IRequest<ProjectDTO>
    {
        public string Name { get; set; }
        public string? Description { get; set; }
    }
}