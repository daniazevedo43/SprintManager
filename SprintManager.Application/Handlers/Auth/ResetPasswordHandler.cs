using MediatR;
using Microsoft.AspNetCore.Identity;
using SprintManager.Application.Commands.Auth;
using SprintManager.Domain.Entities;
using SprintManager.Exceptions.ExceptionsBase;

namespace SprintManager.Application.Handlers.Auth
{
    public class ResetPasswordHandler : IRequestHandler<ResetPasswordCommand>
    {
        private readonly UserManager<User> _userManager;

        public ResetPasswordHandler(UserManager<User> userManager)
        { 
            _userManager = userManager;
        }

        public async Task Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email.ToString());

            if (user == null) throw new SprintManagerNotFoundException("User not found.");

            await _userManager.ResetPasswordAsync(user, request.Token, request.NewPassword);
        }
    }
}