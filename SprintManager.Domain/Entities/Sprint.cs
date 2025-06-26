using SprintManager.Domain.Enums;

namespace SprintManager.Domain.Entities
{
    public class Sprint
    {
        public Guid Id { get; private set; }
        public Guid ProjectId { get; private set; }
        public string Name { get; private set; }
        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }
        public string Description { get; private set; }
        public SprintStatus Status { get; private set; }
        
        public Sprint(Guid projectId, string name, DateTime startDate, DateTime endDate)
        {
            if(projectId == Guid.Empty) throw new ArgumentNullException("This project doesn't exist", nameof(projectId));
            if(string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Project's name can't be null.", nameof(name));
            if(startDate > endDate) throw new ArgumentException("Start date can't be higher than end date", nameof(startDate));

            Id = Guid.NewGuid();
            ProjectId = projectId;
            Name = name;
            StartDate = startDate;
            EndDate = endDate;
            Status = SprintStatus.Planned;
        }

        public Sprint(Guid projectId, string name, DateTime startDate, DateTime endDate, string description)
        {
            if (projectId == Guid.Empty) throw new ArgumentNullException("This project doesn't exist", nameof(projectId));
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Project's name can't be null.", nameof(name));
            if (startDate > endDate) throw new ArgumentException("Start date can't be higher than end date", nameof(startDate));

            Id = Guid.NewGuid();
            ProjectId = projectId;
            Name = name;
            StartDate = startDate;
            EndDate = endDate;
            Description = description;
            Status = SprintStatus.Planned;
        }
    }
}
