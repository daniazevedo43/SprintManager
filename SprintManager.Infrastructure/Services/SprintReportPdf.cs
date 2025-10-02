using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using SprintManager.Application.Interfaces;
using SprintManager.Domain.Entities;

namespace SprintManager.Infrastructure.Services
{
    public class SprintReportPdf : ISprintReportPdf
    {

        public SprintReportPdf()
        {
        }
        
        public IDocument Compose(Sprint sprint, Project project, ICollection<WorkItem> workItems)
        {
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(20));

                    page.Content().Column(column =>
                    {
                        column.Item()
                            .PaddingBottom(42)
                            .Element(c => ComposeHeader(c, sprint, project));

                        column.Item()
                            .Element(c => ComposeContent(c, workItems));
                    });
                });
            });

            return document;
        }

        private void ComposeHeader(IContainer container, Sprint sprint, Project project)
        {
            container.Column(column =>
            {
                column.Spacing(15);

                column.Item()
                    .PaddingBottom(10)
                    .Text($"{sprint.SprintName} - {project.Name}") 
                    .FontSize(23)
                    .Bold();

                column.Item()
                    .Text(text =>
                    {
                        text.Span("Start date: ")
                            .FontSize(15)
                            .Bold();
                        text.Span($"{sprint.StartDate.ToShortDateString()}")
                            .FontSize(15);
                    });

                column.Item()
                    .Text(text =>
                    {
                        text.Span("End date: ")
                            .FontSize(15)
                            .Bold();
                        text.Span($"{sprint.EndDate.ToShortDateString()}")
                            .FontSize(15);
                    });

                if(!string.IsNullOrWhiteSpace(sprint.Description))
                {
                    column.Item()
                        .Text(text =>
                        {
                            text.Span("Description: ")
                                .FontSize(15)
                                .Bold();
                            text.Span(sprint.Description)
                                .FontSize(15);
                        });
                }

                column.Item()
                    .Text(text =>
                    {
                        text.Span("Status: ")
                            .FontSize(15)
                            .Bold();
                        text.Span($"{sprint.Status}")
                            .FontSize(15);
                    });
            });
        }

        private void ComposeContent(IContainer container, ICollection<WorkItem> workItems)
        {
            container.Column(column =>
            {
                column.Item()
                    .PaddingBottom(10)
                    .Text("Work items list")
                    .FontSize(18)
                    .Bold();

                column.Item().Table(table =>
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
                        .FontSize(12)
                        .Bold();

                    table.Cell()
                        .Background(Colors.Grey.Lighten2)
                        .Element(CellStyle)
                        .Text("Type")
                        .FontSize(12)
                        .Bold();

                    table.Cell()
                        .Background(Colors.Grey.Lighten2)
                        .Element(CellStyle)
                        .Text("Assigned user")
                        .FontSize(12)
                        .Bold();

                    table.Cell()
                        .Background(Colors.Grey.Lighten2)
                        .Element(CellStyle)
                        .Text("Priority level")
                        .FontSize(12)
                        .Bold();

                    table.Cell()
                        .Background(Colors.Grey.Lighten2)
                        .Element(CellStyle)
                        .Text("Finish date")
                        .FontSize(12)
                        .Bold();

                    foreach (var workItem in workItems)
                    {
                        table.Cell().Element(CellStyle).Text(workItem.WorkItemTitle);
                        table.Cell().Element(CellStyle).Text(workItem.WorkItemType.ToString());
                        table.Cell().Element(CellStyle).Text(workItem.AssignedUser?.UserName);
                        table.Cell().Element(CellStyle).Text(workItem.PriorityLevel.ToString());
                        table.Cell().Element(CellStyle).Text(workItem.CompletionDate.ToString());
                    }
                });

                static IContainer CellStyle(IContainer container)
                    => container.Border(1).Padding(9).AlignCenter().AlignMiddle();
            });
        }
    }
}