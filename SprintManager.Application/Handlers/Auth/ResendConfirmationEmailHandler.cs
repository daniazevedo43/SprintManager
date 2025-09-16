using MediatR;
using Microsoft.AspNetCore.Identity;
using SprintManager.Application.Commands.Auth;
using SprintManager.Application.Interfaces;
using SprintManager.Domain.Entities;
using System.Net;

namespace SprintManager.Application.Handlers.Auth
{
    public class ResendConfirmationEmailHandler : IRequestHandler<ResendConfirmationEmailCommand>
    {
        private readonly UserManager<User> _userManager;
        private readonly IEmailSender _emailSender;

        public ResendConfirmationEmailHandler(
            UserManager<User> userManager, 
            IEmailSender emailSender
        ) 
        { 
            _userManager = userManager;
            _emailSender = emailSender;
        }

        public async Task Handle(ResendConfirmationEmailCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if(user == null || user.EmailConfirmed) return;

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var encodedToken = WebUtility.UrlEncode(token);
            var confirmationUrl = $"https://localhost:7060/api/Auth/confirm-email?userId={user.Id}&token={encodedToken}";
            var emailContent = $"Welcome to Sprint Manager! Please, <a href='{confirmationUrl}'>click here</a> to confirm your email.";

            if (!string.IsNullOrWhiteSpace(user.Email))
            {
                await _emailSender.SendEmailAsync(user.Email, "Confirm your email", emailContent);
            }
        }
    }
}