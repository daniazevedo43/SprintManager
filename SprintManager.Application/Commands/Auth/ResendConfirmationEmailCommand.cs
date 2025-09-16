using MediatR;

namespace SprintManager.Application.Commands.Auth
{
    public class ResendConfirmationEmailCommand : IRequest
    {
        public string Email { get; set; }
    }
}