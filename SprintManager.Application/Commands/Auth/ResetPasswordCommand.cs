using MediatR;

namespace SprintManager.Application.Commands.Auth
{
    public class ResetPasswordCommand : IRequest
    {
        public Guid UserId { get; set; }
        public string Token { get; set; }
        public string NewPassword { get; set; }
    }
}