using MediatR;
using SprintManager.Domain.Entities;

namespace SprintManager.Application.Commands.Users
{
    public class CreateUserCommand : IRequest<Guid>
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
