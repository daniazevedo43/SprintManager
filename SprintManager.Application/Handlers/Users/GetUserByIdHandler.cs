using AutoMapper;
using MediatR;
using SprintManager.Application.DTOs;
using SprintManager.Application.Interfaces;
using SprintManager.Application.Queries.Users;
using SprintManager.Exceptions.ExceptionsBase;

namespace SprintManager.Application.Handlers.Users
{
    public class GetUserByIdHandler : IRequestHandler<GetUserByIdQuery, UserDTO>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public GetUserByIdHandler(IUserRepository userRepository, IMapper mapper) 
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<UserDTO> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var user = await _userRepository.GetByIdAsync(request.Id);

            if (user == null)
            {
                throw new SprintManagerNotFoundException($"User with ID {user?.Id} not found");
            }

            return _mapper.Map<UserDTO>(user);
        }
    }
}
