using AutoMapper;
using MediatR;
using SprintManager.Application.Commands.Users;
using SprintManager.Application.DTOs;
using SprintManager.Application.Exceptions;
using SprintManager.Application.Interfaces;
using SprintManager.Domain.Entities;

namespace SprintManager.Application.Handlers.Users
{
    public class CreateUserHandler : IRequestHandler<CreateUserCommand, UserDTO>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public CreateUserHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<UserDTO> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var existingUser = await _userRepository.GetByEmailAsync(request.Email);

            if (existingUser != null)
            {
                throw new SprintManagerConflictException($"User with email '{request.Email}' already exists.");
            }

            var user = new User(request.Name, request.Email, request.Password);

            await _userRepository.AddAsync(user);

            return _mapper.Map<UserDTO>(user);
        }
    }
}
