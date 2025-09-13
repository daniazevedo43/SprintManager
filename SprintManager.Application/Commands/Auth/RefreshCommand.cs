using MediatR;
using SprintManager.Application.DTOs;

namespace SprintManager.Application.Commands.Auth
{
    public class RefreshCommand : IRequest<LoginDTO>
    {
        public string RefreshToken { get; set; }
    }
}