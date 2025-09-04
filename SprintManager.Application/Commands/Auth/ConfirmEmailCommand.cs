using MediatR;

namespace SprintManager.Application.Commands.Auth
{
    public class ConfirmEmailCommand : IRequest
    {
        public string UserId { get; set; }
        public string Token { get; set; }
    }
}