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

        public IDocument Compose(string sprintName, DateTime startDate, DateTime endDate, SprintStatus status, string? description)
        {
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(20));

                    page.Header()
                        .PaddingBottom(40)
                        .Element(c => 
                        ComposeHeader(
                            c,
                            sprintName,
                            startDate, 
                            endDate,
                            status,
                            description
                        ));

                    page.Content().Element(ComposeContent);
                });
            });

            return document;
        }

        private void ComposeHeader(IContainer container, string sprintName, DateTime startDate, DateTime endDate, SprintStatus status, string? description)
        {
            container.Column(column =>
            {
                column.Spacing(15);

                column.Item()
                    .PaddingBottom(10)
                    .Text(sprintName) 
                    .FontSize(21)
                    .Bold();

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

                if(!string.IsNullOrWhiteSpace(description))
                {
                    column.Item()
                        .Text(text =>
                        {
                            text.Span("Description: ")
                                .FontSize(15)
                                .Bold();
                            text.Span(description)
                                .FontSize(15);
                        });
                }

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

        private void ComposeContent(IContainer container)
        {
            container.Table(table =>
             {
                 table.ColumnsDefinition(columns =>
                 {
                     columns.RelativeColumn(10);
                     columns.RelativeColumn(10);
                     columns.RelativeColumn(10);
                     columns.RelativeColumn(10);
                     columns.RelativeColumn(10);
                 });

                 table.Cell()
                    .Background(Colors.Grey.Lighten2)
                    .Element(CellStyle)
                    .Text("Title")
                    .FontSize(12);
                
                 table.Cell()
                    .Background(Colors.Grey.Lighten2)
                    .Element(CellStyle)
                    .Text("Type")
                    .FontSize(12);
                 
                 table.Cell()
                    .Background(Colors.Grey.Lighten2)
                    .Element(CellStyle)
                    .Text("Assigned user")
                    .FontSize(12);
                 
                 table.Cell()
                    .Background(Colors.Grey.Lighten2)
                    .Element(CellStyle)
                    .Text("Priority level")
                    .FontSize(12);
                 
                 table.Cell()
                    .Background(Colors.Grey.Lighten2)
                    .Element(CellStyle)
                    .Text("Finish date")
                    .FontSize(12);

                 //table.Cell().Element(CellStyle).Text("150px");
                 //table.Cell().Element(CellStyle).Text("120px");
                 //table.Cell().Element(CellStyle).Text("180px");

                 static IContainer CellStyle(IContainer container)
                     => container.Border(1).Padding(9).AlignCenter().AlignMiddle();
             });
        }
    }
}