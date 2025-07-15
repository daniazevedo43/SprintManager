using AutoMapper;
using MediatR;
using SprintManager.Application.Commands.Users;
using SprintManager.Application.Interfaces;
using SprintManager.Exceptions.ExceptionsBase;

namespace SprintManager.Application.Handlers.Users
{
    public class DeleteUserHandler : IRequestHandler<DeleteUserCommand>
    {
        private readonly IUserRepository _userRepository;

        public DeleteUserHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            if(request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var user = await _userRepository.GetByIdAsync(request.Id);

            if (user == null)
            {
                throw new SprintManagerNotFoundException($"User with ID {request?.Id} not found");
            }

            await _userRepository.DeleteAsync(user);
        }
    }
}