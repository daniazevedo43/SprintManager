using MediatR;

namespace SprintManager.Application.Commands.Auth
{
    public class ForgotPasswordCommand : IRequest
    {
        public string Email { get; set; }
    }
}