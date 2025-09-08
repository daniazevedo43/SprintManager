using MediatR;

namespace SprintManager.Application.Commands.Auth
{
    public class ResetPasswordCommand : IRequest
    {
        public string Email { get; set; }
        public string Token { get; set; }
        public string NewPassword { get; set; }
    }
}