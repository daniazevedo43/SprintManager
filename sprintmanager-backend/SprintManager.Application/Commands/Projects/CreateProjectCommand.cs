using MediatR;
using SprintManager.Application.DTOs;

namespace SprintManager.Application.Commands.Projects
{
    public class CreateProjectCommand : IRequest<ProjectDto>
    {
        public string Name { get; set; }
        public string? Description { get; set; }
    }
}