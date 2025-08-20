using MediatR;

namespace SprintManager.Application.Commands.Images
{
    public class DeleteImageCommand : IRequest
    {
        public Guid Id { get; set; }
    }
}