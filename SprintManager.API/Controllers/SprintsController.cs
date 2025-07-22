using MediatR;
using Microsoft.AspNetCore.Mvc;
using SprintManager.Application.DTOs;
using SprintManager.Application.Queries.Sprints;

namespace SprintManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SprintsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SprintsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<SprintDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllSprints()
        {
            var result = await _mediator.Send(new GetAllSprintsQuery());

            return Ok(result);
        }
    }
}