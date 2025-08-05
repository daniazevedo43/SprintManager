using MediatR;
using Microsoft.AspNetCore.Mvc;
using SprintManager.Application.Commands.Comments;
using SprintManager.Application.DTOs;
using SprintManager.Application.Queries.Comments;

namespace SprintManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CommentsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<CommentDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllComments()
        {
            var result = await _mediator.Send(new GetAllCommentsQuery());

            return Ok(result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(CommentDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCommentById(Guid id)
        {
            var result = await _mediator.Send(new GetCommentByIdQuery { Id = id });

            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(CommentDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> CreateComment(CreateCommentCommand command)
        {
            var result = await _mediator.Send(command);

            return CreatedAtAction(nameof(GetCommentById), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(CommentDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateComment(Guid id, UpdateCommentCommand command)
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
        public async Task<IActionResult> DeleteComment(Guid id)
        {
            await _mediator.Send(new DeleteCommentCommand { Id = id });

            return NoContent();
        }
    }
}