using MediatR;
using Microsoft.AspNetCore.Mvc;
using SprintManager.Application.Commands.Images;
using SprintManager.Application.DTOs;

namespace SprintManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ImagesController(IMediator mediator)
        { 
            _mediator = mediator;
        }

        [HttpPost]
        [ProducesResponseType(typeof(ImageDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AddImage(AddImageCommand command)
        {
            var result = await _mediator.Send(command);

            return CreatedAtAction(null, new { id = result.Id }, result);
        }
    }
}