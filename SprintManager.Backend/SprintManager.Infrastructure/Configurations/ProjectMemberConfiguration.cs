using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SprintManager.Domain.Entities;

namespace SprintManager.Infrastructure.Configurations
{
    public class ProjectMemberConfiguration : IEntityTypeConfiguration<ProjectMember>
    {
        public void Configure(EntityTypeBuilder<ProjectMember> builder)
        {
            builder.HasKey(pm => pm.Id);

            builder.Property(pm => pm.ProjectId)
                .IsRequired();

            builder.Property(pm => pm.UserId)
                .IsRequired();

            builder.Property(pm => pm.Role)
                .IsRequired();
        }
    }
}