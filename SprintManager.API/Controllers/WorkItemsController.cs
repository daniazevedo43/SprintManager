using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SprintManager.Application.Commands.WorkItems;
using SprintManager.Application.DTOs;
using SprintManager.Application.Queries.WorkItems;

namespace SprintManager.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class WorkItemsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public WorkItemsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<WorkItemDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllWorkItems()
        {
            var result = await _mediator.Send(new GetAllWorkItemsQuery());

            return Ok(result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(WorkItemDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetWorkItemById(Guid id)
        {
            var result = await _mediator.Send(new GetWorkItemByIdQuery { Id = id });

            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(WorkItemDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> CreateWorkItem(CreateWorkItemCommand command)
        {
            var result = await _mediator.Send(command);

            return CreatedAtAction(nameof(GetWorkItemById), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(WorkItemDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> UpdateWorkItem(Guid id, UpdateWorkItemCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest("The URL's ID doesn't match the request body's ID");
            }

            var result = await _mediator.Send(command);

            return Ok(result);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteWorkItem(Guid id)
        {
            await _mediator.Send(new DeleteWorkItemCommand { Id = id });

            return NoContent();
        }
    }
}