using MediatR;
using SprintManager.Application.DTOs;

namespace SprintManager.Application.Commands.Auth
{
    public class LoginCommand : IRequest<LoginDTO>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}