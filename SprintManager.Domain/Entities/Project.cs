using SprintManager.Domain.Enums;
using SprintManager.Exceptions.ExceptionsBase;

namespace SprintManager.Domain.Entities
{
    public class Project
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string? Description { get; private set; }
        public DateTime CreationDate { get; private set; }
        public ProjectStatus Status { get; private set; }

        public Project()
        {

        }

        public Project(string name)
        {
            if(string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Project's name can't be null or empty.", nameof(name));
            if(name.Length > 255) throw new SprintManagerTooLongException("Project's name is too long.", 255, name.Length, nameof(name));

            Id = Guid.NewGuid();
            Name = name;
            CreationDate = DateTime.UtcNow;
            Status = ProjectStatus.Active;
        }

        public Project(string name, string? description)
        {
            if(string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Project's name can't be null or empty.", nameof(name));

            if(name.Length > 255) throw new SprintManagerTooLongException("Project's name is too long.", 255, name.Length, nameof(name));
            if(description?.Length > 500) throw new SprintManagerTooLongException("Description is too long.", 500, description.Length, nameof(description));

            Id = Guid.NewGuid();
            Name = name;
            Description = description;
            CreationDate = DateTime.UtcNow;
            Status = ProjectStatus.Active;
        }

        // Update project's name
        public void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Project's name can't be null or empty.", nameof(name));
            if (name.Length > 255) throw new SprintManagerTooLongException("Project's name is too long.", 255, name.Length, nameof(name));

            Name = name;
        }

        // Update project's description
        public void SetDescription(string? description)
        {
            if (description?.Length > 500) throw new SprintManagerTooLongException("Description is too long.", 500, description.Length, nameof(description));

            Description = description;
        }

        // Update project's status
        public void SetStatus(ProjectStatus status)
        {
            Status = status;
        }
    }
}
