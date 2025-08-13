using MediatR;
using SprintManager.Application.DTOs;

namespace SprintManager.Application.Queries.Images
{
    public class GetImageByIdQuery : IRequest<ImageDTO>
    {
        public Guid Id { get; set; }
    }
}