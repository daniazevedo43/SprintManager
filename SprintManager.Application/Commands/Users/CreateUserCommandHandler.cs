using MediatR;
using SprintManager.Application.Exceptions;
using SprintManager.Application.Interfaces;
using SprintManager.Domain.Entities;

namespace SprintManager.Application.Commands.Users
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Guid>
    {
        private readonly IUserRepository _userRepository;

        public CreateUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        
        public async Task<Guid> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var existingUser = await _userRepository.GetByEmailAsync(request.Email);

            if (existingUser == null)
            {
                throw new ConflictException($"User with email '{request.Email}' already exists.");
            }

            var user = new User(request.Name, request.Email, request.Password);

            await _userRepository.AddAsync(user);

            return user.Id;
        }
    }
}
