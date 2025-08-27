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

            builder.Property(u => u.UserName)
                   .IsRequired()     
                   .HasMaxLength(255);

            builder.Property(u => u.Email)
                   .IsRequired()     
                   .HasMaxLength(255);
            
            builder.HasIndex(u => u.Email)
                .IsUnique();

            // Ignore properties
            builder.Ignore(u => u.PhoneNumber);
            builder.Ignore(u => u.PhoneNumberConfirmed);
        }
    }
}