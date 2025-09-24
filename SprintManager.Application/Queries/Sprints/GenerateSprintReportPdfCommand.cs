using MediatR;

namespace SprintManager.Application.Queries.Sprints
{
    public class GenerateSprintReportPdfCommand : IRequest<byte[]>
    {
        public Guid SprintId { get; set; }
    }
}