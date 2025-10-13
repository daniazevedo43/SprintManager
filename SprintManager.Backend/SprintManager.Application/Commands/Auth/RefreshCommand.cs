using MediatR;
using SprintManager.Application.DTOs;

namespace SprintManager.Application.Commands.Auth
{
    public class RefreshCommand : IRequest<LoginDto>
    {
        public string RefreshToken { get; set; }
    }
}