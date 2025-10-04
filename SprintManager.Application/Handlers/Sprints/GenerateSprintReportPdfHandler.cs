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
        private readonly IWorkItemRepository _workItemRepository;
        private readonly ISprintReportPdf _sprintReportPdf;

        public GenerateSprintReportPdfHandler(
            ISprintRepository sprintRepository,
            IProjectRepository projectRepository,
            IWorkItemRepository workItemRepository,
            ISprintReportPdf sprintReportPdf
        )
        {
            _sprintRepository = sprintRepository;
            _projectRepository = projectRepository;
            _workItemRepository = workItemRepository;
            _sprintReportPdf = sprintReportPdf;
        }

        public async Task<PdfFileDTO> Handle(GenerateSprintReportPdfCommand request, CancellationToken cancellationToken)
        {
            var sprint = await _sprintRepository.GetByIdAsync(request.SprintId);

            if (sprint == null) throw new SprintManagerNotFoundException($"Sprint with ID {request.SprintId} not found.");

            var project = await _projectRepository.GetByIdAsync(sprint.ProjectId);

            if (project == null) throw new SprintManagerNotFoundException($"Project with ID {sprint.ProjectId} not found.");

            var workItems = await _workItemRepository.GetWorkItemsBySprintIdAsync(request.SprintId);

            var document = _sprintReportPdf.Compose(sprint, project, workItems);

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