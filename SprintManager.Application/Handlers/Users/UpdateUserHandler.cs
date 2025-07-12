using AutoMapper;
using MediatR;
using SprintManager.Application.Commands.Users;
using SprintManager.Application.DTOs;
using SprintManager.Application.Interfaces;
using SprintManager.Domain.Entities;
using SprintManager.Exceptions.ExceptionsBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var user = await _userRepository.GetByIdAsync(request.Id);

            if (user == null)
            {
                throw new SprintManagerNotFoundException($"User with ID {request?.Id} not found");
            }

            user?.SetName(request.Name);
            user?.SetEmail(request.Email);
            user?.SetPassword(request.Password);

            await _userRepository.UpdateAsync(user);

            return _mapper.Map<UserDTO>(user);
        }
    }
}
