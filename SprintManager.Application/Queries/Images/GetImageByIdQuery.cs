using MediatR;
using SprintManager.Application.DTOs;

namespace SprintManager.Application.Queries.Images
{
    public class GetImageByIdQuery : IRequest<ImageDto>
    {
        public Guid Id { get; set; }
    }
}