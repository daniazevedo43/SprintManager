using MediatR;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using SprintManager.Application.Interfaces;
using SprintManager.Application.Queries.Sprints;
using SprintManager.Exceptions.ExceptionsBase;

namespace SprintManager.Application.Handlers.Sprints
{
    public class GenerateSprintReportPdfHandler : IRequestHandler<GenerateSprintReportPdfCommand, byte[]>
    {
        private readonly ISprintRepository _sprintRepository;

        public GenerateSprintReportPdfHandler(ISprintRepository sprintRepository)
        {
            _sprintRepository = sprintRepository;
        }

        public async Task<byte[]> Handle(GenerateSprintReportPdfCommand request, CancellationToken cancellationToken)
        {
            var sprint = await _sprintRepository.GetByIdAsync(request.SprintId);

            if (sprint == null) throw new SprintManagerNotFoundException($"Sprint with ID {request.SprintId} not found.");

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

            var pdfBytes = document.GeneratePdf();

            return pdfBytes;
        }
    }
}
