using MediatR;
using Microsoft.AspNetCore.Mvc;
using SprintManager.Application.Commands.Projects;
using SprintManager.Application.Commands.Users;
using SprintManager.Application.DTOs;
using SprintManager.Application.Queries.Projects;

namespace SprintManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProjectsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<ProjectDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllProjects()
        {
            var result = await _mediator.Send(new GetAllProjectsQuery());

            return Ok(result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(List<ProjectDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProjectById(Guid id)
        {
            var result = await _mediator.Send(new GetProjectByIdQuery { Id = id });

            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ProjectDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> CreateUser(CreateProjectCommand command)
        {
            var result = await _mediator.Send(command);

            return CreatedAtAction(nameof(GetProjectById), new { id = result.Id }, result);
        }
    }
}