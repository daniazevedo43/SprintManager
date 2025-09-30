using MediatR;
using QuestPDF.Fluent;
using SprintManager.Application.DTOs;
using SprintManager.Application.Interfaces;
using SprintManager.Application.Queries.Sprints;
using SprintManager.Exceptions.ExceptionsBase;

namespace SprintManager.Application.Handlers.Sprints
{
    public class GenerateSprintReportPdfHandler : IRequestHandler<GenerateSprintReportPdfCommand, PdfFileDTO>
    {
        private readonly ISprintRepository _sprintRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly ISprintReportPdf _sprintReportPdf;

        public GenerateSprintReportPdfHandler(
            ISprintRepository sprintRepository,
            IProjectRepository projectRepository,
            ISprintReportPdf sprintReportPdf
        )
        {
            _sprintRepository = sprintRepository;
            _projectRepository = projectRepository;
            _sprintReportPdf = sprintReportPdf;
        }

        public async Task<PdfFileDTO> Handle(GenerateSprintReportPdfCommand request, CancellationToken cancellationToken)
        {
            var sprint = await _sprintRepository.GetByIdAsync(request.SprintId);

            if (sprint == null) throw new SprintManagerNotFoundException($"Sprint with ID {request.SprintId} not found.");

            var project = await _projectRepository.GetByIdAsync(sprint.ProjectId);

            var document = _sprintReportPdf.Compose(
                sprint.SprintName, 
                sprint.StartDate, 
                sprint.EndDate,
                sprint.Description,
                sprint.Status
            );

            var fileName = $"{project?.Name.Replace(" ", "_")}_{sprint.SprintName.Replace(" ", "_")}_report".ToLower();
            var fileBytes = document.GeneratePdf();

            return new PdfFileDTO
            { 
                FileName = fileName,
                FileBytes = fileBytes
            };
        }
    }
}