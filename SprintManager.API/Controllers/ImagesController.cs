using MediatR;
using Microsoft.AspNetCore.Mvc;
using SprintManager.Application.Commands.Images;
using SprintManager.Application.DTOs;
using SprintManager.Application.Queries.Images;

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

        [HttpGet]
        [ProducesResponseType(typeof(List<ImageDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllImages()
        {
            var result = await _mediator.Send(new GetAllImagesQuery());

            return Ok(result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ImageDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetImageById(Guid id)
        {
            var result = await _mediator.Send(new GetImageByIdQuery { Id = id });

            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ImageDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
        public async Task<IActionResult> AddImage(AddImageCommand command)
        {
            var result = await _mediator.Send(command);

            return CreatedAtAction(nameof(GetImageById), new { id = result.Id }, result);
        }
    }
}