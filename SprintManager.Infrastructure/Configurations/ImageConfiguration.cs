using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SprintManager.Domain.Entities;

namespace SprintManager.Infrastructure.Configurations
{
    public class ImageConfiguration : IEntityTypeConfiguration<Image>
    {
        public void Configure(EntityTypeBuilder<Image> builder)
        {
            builder.HasKey(i => i.Id);

            builder.Property(i => i.WorkItemId)
                .IsRequired();

            builder.Property(i => i.UserId)
                .IsRequired();

            builder.Property(i => i.ContentType)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(i => i.FileName)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(i => i.FilePath)
                .HasMaxLength(500)
                .IsRequired();

            builder.Property(i => i.AttachmentDate)
                .IsRequired();
        }
    }
}