using MediatR;

namespace SprintManager.Application.Commands.Images
{
    public class RemoveImageCommand : IRequest
    {
        public Guid Id { get; set; }
    }
}