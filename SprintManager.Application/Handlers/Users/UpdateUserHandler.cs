using AutoMapper;
using MediatR;
using SprintManager.Application.Commands.Users;
using SprintManager.Application.DTOs;
using SprintManager.Application.Exceptions;
using SprintManager.Application.Interfaces;
using SprintManager.Exceptions.ExceptionsBase;

namespace SprintManager.Application.Handlers.Users
{
    public class UpdateUserHandler : IRequestHandler<UpdateUserCommand, UserDTO>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UpdateUserHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<UserDTO> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.Id);
            var existingEmail = await _userRepository.GetByEmailAsync(request.Email);

            if (user == null) throw new SprintManagerNotFoundException($"User with ID {request?.Id} not found");

            if (existingEmail != null && user.Email != request.Email) throw new SprintManagerConflictException($"A user with email '{request.Email}' already exists.");

            user?.SetName(request.Name);
            user?.SetEmail(request.Email);

            await _userRepository.UpdateAsync(user);

            return _mapper.Map<UserDTO>(user);
        }
    }
}