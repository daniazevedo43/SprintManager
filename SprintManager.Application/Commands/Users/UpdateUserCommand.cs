using MediatR;
using SprintManager.Application.DTOs;

namespace SprintManager.Application.Commands.Users
{
    public class UpdateUserCommand : IRequest<UserDTO>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
