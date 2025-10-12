using MediatR;
using SprintManager.Application.DTOs;

namespace SprintManager.Application.Queries.Images
{
    public class GetAllImagesQuery : IRequest<List<ImageDto>>
    {
    }
}