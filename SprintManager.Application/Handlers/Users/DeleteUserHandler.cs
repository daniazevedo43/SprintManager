using AutoMapper;
using MediatR;
using SprintManager.Application.Commands.Users;
using SprintManager.Application.Interfaces;
using SprintManager.Exceptions.ExceptionsBase;

namespace SprintManager.Application.Handlers.Users
{
    public class DeleteUserHandler : IRequestHandler<DeleteUserCommand, bool>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public DeleteUserHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
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

            return true;
        }
    }
}