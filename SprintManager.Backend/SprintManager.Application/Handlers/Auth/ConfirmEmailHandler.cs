using MediatR;
using Microsoft.AspNetCore.Identity;
using SprintManager.Application.Queries.Auth;
using SprintManager.Domain.Entities;
using SprintManager.Exceptions.ExceptionsBase;

namespace SprintManager.Application.Handlers.Auth
{
    public class ConfirmEmailHandler : IRequestHandler<ConfirmEmailQuery>
    {
        private readonly UserManager<User> _userManager;

        public ConfirmEmailHandler(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task Handle(ConfirmEmailQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId.ToString());

            if (user == null) throw new SprintManagerNotFoundException("User not found.");

            await _userManager.ConfirmEmailAsync(user, request.Token);
        }
    }
}