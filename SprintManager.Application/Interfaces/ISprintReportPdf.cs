using QuestPDF.Fluent;

namespace SprintManager.Application.Interfaces
{
    public interface ISprintReportPdf
    {
        Document Compose();
    }
}
