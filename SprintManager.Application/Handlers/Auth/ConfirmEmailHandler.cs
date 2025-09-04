using MediatR;
using Microsoft.AspNetCore.Identity;
using SprintManager.Application.Commands.Auth;
using SprintManager.Domain.Entities;
using SprintManager.Exceptions.ExceptionsBase;

namespace SprintManager.Application.Handlers.Auth
{
    public class ConfirmEmailHandler : IRequestHandler<ConfirmEmailCommand>
    {
        private readonly UserManager<User> _userManager;

        public ConfirmEmailHandler(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);

            if (user == null) throw new SprintManagerNotFoundException("User not found.");

            await _userManager.ConfirmEmailAsync(user, request.Token);
        }
    }
}