using MediatR;

namespace SprintManager.Application.Queries.Auth
{
    public class ConfirmEmailQuery : IRequest
    {
        public Guid UserId { get; set; }
        public string Token { get; set; }
    }
}