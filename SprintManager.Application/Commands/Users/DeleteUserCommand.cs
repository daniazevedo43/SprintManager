using MediatR;

namespace SprintManager.Application.Commands.Users
{
    public class DeleteUserCommand : IRequest
    {
        public Guid Id { get; set; }
    }
}