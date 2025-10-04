using MediatR;
using SprintManager.Application.DTOs;

namespace SprintManager.Application.Queries.Sprints
{
    public class GenerateSprintReportPdfCommand : IRequest<PdfFileDTO>
    {
        public Guid SprintId { get; set; }
    }
}