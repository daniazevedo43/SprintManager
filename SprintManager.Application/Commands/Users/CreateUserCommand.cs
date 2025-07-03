using MediatR;
using SprintManager.Application.DTOs;

namespace SprintManager.Application.Commands.Users
{
    public class CreateUserCommand : IRequest<UserDTO>
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
