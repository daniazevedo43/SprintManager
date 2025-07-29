using MediatR;
using Microsoft.AspNetCore.Mvc;
using SprintManager.Application.DTOs;
using SprintManager.Application.Queries.WorkItems;

namespace SprintManager.API.Controllers
{
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
    }
}