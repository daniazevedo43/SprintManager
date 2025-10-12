using MediatR;

namespace SprintManager.Application.Commands.Auth
{
    public class DeleteAccountCommand : IRequest
    {
        public string Password { get; set; }
    }
}