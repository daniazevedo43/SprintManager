using MediatR;
using Microsoft.AspNetCore.Mvc;
using SprintManager.Application.Commands.ProjectMembers;
using SprintManager.Application.DTOs;
using SprintManager.Application.Queries.ProjectMembers;

namespace SprintManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectMembersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProjectMembersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<ProjectMemberDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllProjectMembers()
        {
            var result = await _mediator.Send(new GetAllProjectMembersQuery());

            return Ok(result);
        }

        [HttpGet("{projectId}")]
        [ProducesResponseType(typeof(List<ProjectMemberBasicDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetMembersByProjectId(Guid projectId)
        {
            var result = await _mediator.Send(new GetProjectMembersByProjectIdQuery { ProjectId = projectId });

            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ProjectMemberDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> AddProjectMember(AddProjectMemberCommand command)
        {
            var result = await _mediator.Send(command);

            return StatusCode(StatusCodes.Status201Created, result);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RemoveProjectMember(Guid id)
        {
            await _mediator.Send(new RemoveProjectMemberCommand { Id = id });

            return NoContent();
        }
    }
}