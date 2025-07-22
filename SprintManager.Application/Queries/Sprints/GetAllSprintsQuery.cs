using MediatR;
using SprintManager.Application.DTOs;

namespace SprintManager.Application.Queries.Sprints
{
    public class GetAllSprintsQuery : IRequest<List<SprintDTO>>
    {
    }
}