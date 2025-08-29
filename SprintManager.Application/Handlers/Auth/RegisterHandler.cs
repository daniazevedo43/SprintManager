using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using SprintManager.Application.Commands.Auth;
using SprintManager.Application.DTOs;
using SprintManager.Application.Exceptions;
using SprintManager.Domain.Entities;

namespace SprintManager.Application.Handlers.Auth
{
    public class RegisterHandler : IRequestHandler<RegisterCommand, UserDTO>
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public RegisterHandler(UserManager<User> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<UserDTO> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var existingUsername = await _userManager.FindByNameAsync(request.UserName);
            var existingEmail = await _userManager.FindByEmailAsync(request.Email);

            if (existingUsername != null) throw new SprintManagerConflictException($"A user with username '{request.UserName}' already exists.");
            if (existingEmail != null) throw new SprintManagerConflictException($"A user with email '{request.Email}' already exists.");

            var user = new User(request.Name, request.UserName, request.Email, request.Password);

            await _userManager.CreateAsync(user, request.Password);

            return _mapper.Map<UserDTO>(user);
        }
    }
}