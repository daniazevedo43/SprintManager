using MediatR;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
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

        public GenerateSprintReportPdfHandler(
            ISprintRepository sprintRepository,
            IProjectRepository projectRepository
        )
        {
            _sprintRepository = sprintRepository;
            _projectRepository = projectRepository;
        }

        public async Task<PdfFileDTO> Handle(GenerateSprintReportPdfCommand request, CancellationToken cancellationToken)
        {
            var sprint = await _sprintRepository.GetByIdAsync(request.SprintId);

            if (sprint == null) throw new SprintManagerNotFoundException($"Sprint with ID {request.SprintId} not found.");

            var project = await _projectRepository.GetByIdAsync(sprint.ProjectId);

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, QuestPDF.Infrastructure.Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(20));

                    page.Header()
                        .Text("Hello PDF!")
                        .SemiBold().FontSize(36).FontColor(Colors.Blue.Medium);

                    page.Content()
                        .PaddingVertical(1, QuestPDF.Infrastructure.Unit.Centimetre)
                        .Column(x =>
                        {
                            x.Spacing(20);

                            x.Item().Text(Placeholders.LoremIpsum());
                            x.Item().Image(Placeholders.Image(200, 100));
                        });

                    page.Footer()
                        .AlignCenter()
                        .Text(x =>
                        {
                            x.Span("Page ");
                            x.CurrentPageNumber();
                        });
                });
            });

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