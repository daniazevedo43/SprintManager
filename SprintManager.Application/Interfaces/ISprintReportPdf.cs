using QuestPDF.Infrastructure;
using SprintManager.Domain.Entities;

namespace SprintManager.Application.Interfaces
{
    public interface ISprintReportPdf
    {
        IDocument Compose(Sprint sprint, Project project, ICollection<WorkItem> workItems);
    }
}