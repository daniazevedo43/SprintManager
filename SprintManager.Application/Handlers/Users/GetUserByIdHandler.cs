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

        public GetUserByIdHandler(IUserRepository userRepository) 
        {
            _userRepository = userRepository;
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

            return new UserDTO
            { 
                Id = user.Id,
                Name = user.Name,
                Email = user.Email
            };
        }
    }
}
