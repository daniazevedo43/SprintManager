using MediatR;
using SprintManager.Application.DTOs;

namespace SprintManager.Application.Queries.Users
{
    public class GetAllUsersQuery : IRequest<List<UserDTO>>
    {
    }
}
