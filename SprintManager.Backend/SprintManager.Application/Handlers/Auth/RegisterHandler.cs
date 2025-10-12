using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using SprintManager.Application.Commands.Auth;
using SprintManager.Application.DTOs;
using SprintManager.Application.Exceptions;
using SprintManager.Application.Interfaces;
using SprintManager.Domain.Entities;
using System.Net;

namespace SprintManager.Application.Handlers.Auth
{
    public class RegisterHandler : IRequestHandler<RegisterCommand, UserDto>
    {
        private readonly UserManager<User> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly IMapper _mapper;

        public RegisterHandler(
            UserManager<User> userManager, 
            IEmailSender emailSender, 
            IMapper mapper
        )
        {
            _userManager = userManager;
            _emailSender = emailSender;
            _mapper = mapper;
        }

        public async Task<UserDto> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var existingUsername = await _userManager.FindByNameAsync(request.UserName);
            var existingEmail = await _userManager.FindByEmailAsync(request.Email);

            if (existingUsername != null) throw new SprintManagerConflictException($"A user with username '{request.UserName}' already exists.");
            if (existingEmail != null) throw new SprintManagerConflictException($"A user with email '{request.Email}' already exists.");

            var user = new User(request.Name, request.UserName, request.Email, request.Password);

            await _userManager.CreateAsync(user, request.Password);

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var encodedToken = WebUtility.UrlEncode(token);
            var confirmationUrl = $"https://localhost:7060/api/Auth/confirm-email?userId={user.Id}&token={encodedToken}";
            var emailContent = $"Welcome to Sprint Manager! Please, <a href='{confirmationUrl}'>click here</a> to confirm your email.";

            if (!string.IsNullOrWhiteSpace(user.Email))
            { 
                await _emailSender.SendEmailAsync(user.Email, "Confirm your email", emailContent);
            }

            return _mapper.Map<UserDto>(user);
        }
    }
}