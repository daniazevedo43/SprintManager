using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SprintManager.Domain.Entities;

namespace SprintManager.Infrastructure.Configurations
{
    public class ProjectConfiguration : IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Name)
                   .IsRequired()
                   .HasMaxLength(255);

            builder.Property(p => p.Description)
                .HasMaxLength(500);

            builder.Property(p => p.CreationDate)
                .IsRequired();

            builder.Property(p => p.Status)
                .IsRequired();
        }
    }
}
