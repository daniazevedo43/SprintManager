using MediatR;
using Microsoft.AspNetCore.Mvc;
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
    }
}