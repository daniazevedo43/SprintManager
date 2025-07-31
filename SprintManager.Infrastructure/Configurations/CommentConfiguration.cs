using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SprintManager.Domain.Entities;

namespace SprintManager.Infrastructure.Configurations
{
    public class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.WorkItemId)
                .IsRequired();

            builder.Property(c => c.UserId)
                .IsRequired();

            builder.Property(c => c.Text)
                .HasMaxLength(500)
                .IsRequired();

            builder.Property(c => c.CreationDate)
                .IsRequired();
        }
    }
}