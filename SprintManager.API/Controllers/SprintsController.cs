using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SprintManager.Application.Commands.Sprints;
using SprintManager.Application.DTOs;
using SprintManager.Application.Queries.Sprints;

namespace SprintManager.API.Controllers
{
    [Authorize]
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
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAllSprints()
        {
            var result = await _mediator.Send(new GetAllSprintsQuery());

            return Ok(result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(SprintDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetSprintById(Guid id)
        {
            var result = await _mediator.Send(new GetSprintByIdQuery { Id = id });

            return Ok(result);
        }

        [HttpGet("{id}/report")]
        [ProducesResponseType(typeof(PdfFileDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GenerateSprintReportPdf(Guid id)
        {
            var result = await _mediator.Send(new GenerateSprintReportPdfCommand { SprintId = id });

            return File(result.FileBytes, "application/pdf", result.FileName);
        }

        [HttpPost]
        [ProducesResponseType(typeof(SprintDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> CreateSprint(CreateSprintCommand command)
        {
            var result = await _mediator.Send(command);

            return CreatedAtAction(nameof(GetSprintById), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(SprintDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> UpdateSprint(Guid id, UpdateSprintCommand command)
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
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteSprint(Guid id)
        {
            await _mediator.Send(new DeleteSprintCommand { Id = id });

            return NoContent();
        }
    }
}