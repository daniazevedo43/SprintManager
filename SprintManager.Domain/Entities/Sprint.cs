using SprintManager.Domain.Enums;
using SprintManager.Exceptions.ExceptionsBase;

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

        public Sprint()
        {

        }
        
        public Sprint(Guid projectId, string name, DateTime startDate, DateTime endDate)
        {
            if(projectId == Guid.Empty) throw new ArgumentException("Project ID can't be null or empty.", nameof(projectId));
            if(string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Sprint's name can't be null or empty.", nameof(name));
            if(startDate > endDate) throw new SprintManagerInvalidDateRangeException("Start date can't be higher than end date", startDate, endDate, nameof(startDate));

            if(name.Length > 255) throw new SprintManagerTooLongException("Sprint's name can't exceed 255 characters.", 255, name.Length, nameof(name));

            Id = Guid.NewGuid();
            ProjectId = projectId;
            Name = name;
            StartDate = startDate.ToUniversalTime();
            EndDate = endDate.ToUniversalTime();
            Status = SprintStatus.Planned;
        }

        public Sprint(Guid projectId, string name, DateTime startDate, DateTime endDate, string description)
        {
            if(projectId == Guid.Empty) throw new ArgumentException("Project ID can't be null or empty.", nameof(projectId));
            if(string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Sprint's name can't be null or empty.", nameof(name));
            if(startDate > endDate) throw new SprintManagerInvalidDateRangeException("Start date can't be higher than end date", startDate, endDate, nameof(startDate));

            if(name.Length > 255) throw new SprintManagerTooLongException("Sprint's name can't exceed 255 characters.", 255, name.Length, nameof(name));
            if(description.Length > 500) throw new SprintManagerTooLongException("Sprint's description can't exceed 500 characters.", 500, description.Length, nameof(description));

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
            if(string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Sprint's name can't be null or empty.", nameof(name));
            if(name.Length > 255) throw new SprintManagerTooLongException("Sprint's name can't exceed 255 characters.", 255, name.Length, nameof(name));

            Name = name;
        }

        // Update sprint's start date
        public void SetStartDate(DateTime startDate)
        {
            if(startDate > EndDate) throw new SprintManagerInvalidDateRangeException("Start date can't be higher than end date", startDate, EndDate, nameof(startDate));

            StartDate = startDate.ToUniversalTime();
        }

        // Update sprint's end date
        public void SetEndDate(DateTime endDate)
        {
            if(StartDate > endDate) throw new SprintManagerInvalidDateRangeException("Start date can't be higher than end date", StartDate, endDate, nameof(endDate));

            EndDate = endDate.ToUniversalTime();
        }

        // Update sprint's description
        public void SetDescription(string description)
        {
            if (description.Length > 500) throw new SprintManagerTooLongException("Sprint's description can't exceed 500 characters.", 500, description.Length, nameof(description));

            Description = description;
        }

        // Update sprint's status
        public void SetStatus(SprintStatus status)
        {
            Status = status;
        }
    }
}
