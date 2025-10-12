using MediatR;
using SprintManager.Application.DTOs;

namespace SprintManager.Application.Queries.Sprints
{
    public class GenerateSprintReportPdfCommand : IRequest<PdfFileDto>
    {
        public Guid SprintId { get; set; }
    }
}