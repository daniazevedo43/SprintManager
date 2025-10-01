using QuestPDF.Infrastructure;
using SprintManager.Domain.Entities;

namespace SprintManager.Application.Interfaces
{
    public interface ISprintReportPdf
    {
        IDocument Compose(Sprint sprint, ICollection<WorkItem> workItems);
    }
}