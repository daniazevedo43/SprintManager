using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SprintManager.Domain.Entities;

namespace SprintManager.Infrastructure.Configurations
{
    public class SprintConfiguration : IEntityTypeConfiguration<Sprint>
    {
        public void Configure(EntityTypeBuilder<Sprint> builder)
        {
            builder.HasKey(s => s.Id);

            builder.Property(s => s.ProjectId)
                .IsRequired();

            builder.Property(s => s.SprintName)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(s => s.StartDate)
                .IsRequired();

            builder.Property(s => s.EndDate)
                .IsRequired();

            builder.Property(s => s.Description)
                .HasMaxLength(500);

            builder.Property(s => s.Status)
                .IsRequired();
        }
    }
}
