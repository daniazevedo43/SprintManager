using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SprintManager.Domain.Entities;

namespace SprintManager.Infrastructure.Configurations
{
    public class WorkItemConfiguration : IEntityTypeConfiguration<WorkItem>
    {
        public void Configure(EntityTypeBuilder<WorkItem> builder)
        {
            builder.HasKey(w => w.Id);

            builder.Property(w => w.ProjectId)
                .IsRequired();

            builder.Property(w => w.SprintId);

            builder.Property(w => w.UserId);

            builder.Property(w => w.WorkItemType)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(w => w.Title)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(w => w.Description)
                .HasMaxLength(500);

            builder.Property(w => w.Status)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(w => w.PriorityLevel);

            builder.Property(w => w.CreationDate)
                .IsRequired();

            builder.Property(w => w.CompletionDate);

            builder.Property(w => w.HoursEstimate);
        }
    }
}