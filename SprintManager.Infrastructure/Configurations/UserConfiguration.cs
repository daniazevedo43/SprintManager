using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SprintManager.Domain.Entities;

namespace SprintManager.Infrastructure.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id);

            builder.Property(u => u.Name)
                   .IsRequired()     
                   .HasMaxLength(255);

            builder.Property(u => u.Email)
                   .IsRequired()     
                   .HasMaxLength(255);
            
            builder.HasIndex(u => u.Email)
                .IsUnique();    

            builder.Property(u => u.PasswordHash)
                .IsRequired()
                .HasMaxLength(64);
        }
    }
}
