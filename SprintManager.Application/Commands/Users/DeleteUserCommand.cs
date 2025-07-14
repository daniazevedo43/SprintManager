using MediatR;

namespace SprintManager.Application.Commands.Users
{
    public class DeleteUserCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
    }
}