using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using SprintManager.Application.Interfaces;
using SprintManager.Domain.Enums;

namespace SprintManager.Infrastructure.Services
{
    public class SprintReportPdf : ISprintReportPdf
    {

        public SprintReportPdf() 
        { 
        }

        public IDocument Compose(string sprintName, DateTime startDate, DateTime endDate, string description, SprintStatus status)
        {
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(20));

                    page.Header().Element(c => 
                        ComposeHeader(
                            c,
                            sprintName,
                            startDate, 
                            endDate,
                            description,
                            status
                        ));
                });
            });

            return document;
        }

        private void ComposeHeader(IContainer container, string sprintName, DateTime startDate, DateTime endDate, string description, SprintStatus status)
        {
            container.Column(column =>
            {
                column.Spacing(15);

                column.Item()
                    .Text(sprintName)
                    .FontSize(21)
                    .Bold();

                column.Item()
                    .Height(10);

                column.Item()
                    .Text(text =>
                    {
                        text.Span("Start date: ")
                            .FontSize(15)
                            .Bold();
                        text.Span($"{startDate.ToShortDateString()}")
                            .FontSize(15);
                    });

                column.Item()
                    .Text(text =>
                    {
                        text.Span("End date: ")
                            .FontSize(15)
                            .Bold();
                        text.Span($"{endDate.ToShortDateString()}")
                            .FontSize(15);
                    });

                column.Item()
                    .Text(text =>
                    {
                        text.Span("Description: ")
                            .FontSize(15)
                            .Bold();
                        text.Span(description)
                            .FontSize(15);
                    });

                column.Item()
                    .Text(text =>
                    {
                        text.Span("Status: ")
                            .FontSize(15)
                            .Bold();
                        text.Span($"{status}")
                            .FontSize(15);
                    });
            });
        }
    }
}