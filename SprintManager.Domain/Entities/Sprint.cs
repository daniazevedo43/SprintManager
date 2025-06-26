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
        public string? Description { get; private set; }
        public SprintStatus Status { get; private set; }
        
        public Sprint(Guid projectId, string name, DateTime startDate, DateTime endDate)
        {
            if(projectId == Guid.Empty) throw new ArgumentException("This project doesn't exist", nameof(projectId));
            if(string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Project's name can't be null.", nameof(name));
            if(startDate > endDate) throw new ArgumentException("Start date can't be higher than end date", nameof(startDate));

            if(name.Length > 255) throw new ArgumentException("Sprint's name can't exceed 255 characters.", nameof(name));

            Id = Guid.NewGuid();
            ProjectId = projectId;
            Name = name;
            StartDate = startDate.ToUniversalTime();
            EndDate = endDate.ToUniversalTime();
            Status = SprintStatus.Planned;
        }

        public Sprint(Guid projectId, string name, DateTime startDate, DateTime endDate, string? description)
        {
            if(projectId == Guid.Empty) throw new ArgumentException("This project doesn't exist", nameof(projectId));
            if(string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Project's name can't be null.", nameof(name));
            if(startDate > endDate) throw new ArgumentException("Start date can't be higher than end date", nameof(startDate));

            if(name.Length > 255) throw new ArgumentException("Sprint's name can't exceed 255 characters.", nameof(name));
            if(description?.Length > 500) throw new ArgumentException("Sprint's description can't exceed 500 characters.", nameof(description));

            Id = Guid.NewGuid();
            ProjectId = projectId;
            Name = name;
            StartDate = startDate.ToUniversalTime();
            EndDate = endDate.ToUniversalTime();
            Description = description;
            Status = SprintStatus.Planned;
        }

        // Update sprint's name
        public void SetName(string name)
        {
            if(string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Project's name can't be null.", nameof(name));
            if (name.Length > 255) throw new ArgumentException("Sprint's name can't exceed 255 characters.", nameof(name));

            Name = name;
        }

        // Update sprint's start date
        public void SetStartDate(DateTime startDate)
        { 
            if(startDate > EndDate) throw new ArgumentException("Start date can't be higher than end date", nameof(startDate));

            StartDate = startDate.ToUniversalTime();
        }

        // Update sprint's end date
        public void SetEndDate(DateTime endDate)
        {
            if(StartDate > endDate) throw new ArgumentException("Start date can't be higher than end date", nameof(endDate));

            EndDate = endDate.ToUniversalTime();
        }

        // Update sprint's description
        public void SetDescription(string description)
        {
            if (description?.Length > 500) throw new ArgumentException("Sprint's description can't exceed 500 characters.", nameof(description));

            Description = description;
        }

        // Update sprint's status
        public void SetStatus(SprintStatus status)
        {
            Status = status;
        }
    }
}
