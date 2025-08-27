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
            var existingUser = await _userManager.FindByEmailAsync(request.Email);

            if (existingUser != null) throw new SprintManagerConflictException($"A user with email '{request.Email}' already exists.");

            var user = new User(request.UserName, request.Email, request.Password);

            await _userManager.CreateAsync(user, request.Password);

            return _mapper.Map<UserDTO>(user);
        }
    }
}