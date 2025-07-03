using MediatR;
using SprintManager.Application.Commands.Users;
using SprintManager.Application.Exceptions;
using SprintManager.Application.Interfaces;
using SprintManager.Domain.Entities;

namespace SprintManager.Application.Handlers.Users
{
    public class CreateUserHandler : IRequestHandler<CreateUserCommand, Guid>
    {
        private readonly IUserRepository _userRepository;

        public CreateUserHandler(IUserRepository userRepository)
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

            if (existingUser != null)
            {
                throw new SprintManagerConflictException($"User with email '{request.Email}' already exists.");
            }

            var user = new User(request.Name, request.Email, request.Password);

            await _userRepository.AddAsync(user);

            return user.Id;
        }
    }
}
