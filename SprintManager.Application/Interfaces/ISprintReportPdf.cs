using QuestPDF.Infrastructure;
using SprintManager.Domain.Enums;

namespace SprintManager.Application.Interfaces
{
    public interface ISprintReportPdf
    {
        IDocument Compose(string sprintName, DateTime startDate, DateTime endDate, string description, SprintStatus status);
    }
}