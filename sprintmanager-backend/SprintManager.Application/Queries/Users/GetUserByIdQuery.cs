using MediatR;
using SprintManager.Application.DTOs;

namespace SprintManager.Application.Queries.Users
{
    public class GetUserByIdQuery : IRequest<UserDto>
    {
        public Guid Id { get; set; }
    }
}
