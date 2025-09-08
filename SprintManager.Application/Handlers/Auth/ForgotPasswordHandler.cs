using MediatR;
using Microsoft.AspNetCore.Identity;
using SprintManager.Application.Commands.Auth;
using SprintManager.Application.Interfaces;
using SprintManager.Domain.Entities;

namespace SprintManager.Application.Handlers.Auth
{
    public class ForgotPasswordHandler : IRequestHandler<ForgotPasswordCommand>
    {
        private readonly UserManager<User> _userManager;
        private readonly IEmailSender _emailSender;

        public ForgotPasswordHandler(UserManager<User> userManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            _emailSender = emailSender;
        }

        public async Task Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null) return;

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var emailContent = $"Please, use the following token to reset your password: {token}";

            await _emailSender.SendEmailAsync(request.Email, "Reset Password", emailContent);
        }
    }
}