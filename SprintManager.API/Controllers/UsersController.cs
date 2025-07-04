using MediatR;
using Microsoft.AspNetCore.Mvc;
using SprintManager.Application.DTOs;
using SprintManager.Application.Queries.Users;
using SprintManager.Exceptions.ExceptionsBase;

namespace SprintManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(UserDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var result = await _mediator.Send(new GetUserByIdQuery { Id = id });

                return Ok(result);
            }
            catch(SprintManagerNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
