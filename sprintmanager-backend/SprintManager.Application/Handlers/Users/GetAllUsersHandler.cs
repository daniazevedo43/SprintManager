using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SprintManager.Application.DTOs;
using SprintManager.Application.Queries.Users;
using SprintManager.Domain.Entities;

namespace SprintManager.Application.Handlers.Users
{
    public class GetAllUsersHandler : IRequestHandler<GetAllUsersQuery, List<UserDto>>
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public GetAllUsersHandler(UserManager<User> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<List<UserDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            var users = await _userManager.Users
                .OrderBy(u => u.UserName)
                .ToListAsync(cancellationToken);

            return _mapper.Map<List<UserDto>>(users);
        }
    }
}