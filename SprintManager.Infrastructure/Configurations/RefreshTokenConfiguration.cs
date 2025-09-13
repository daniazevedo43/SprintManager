using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SprintManager.Domain.Entities;

namespace SprintManager.Infrastructure.Configurations
{
    public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.UserId)
               .IsRequired();

            builder.Property(t => t.Token)
                .IsRequired();

            builder.Property(t => t.ExpirationDate)
                .IsRequired();

            builder.Property(t => t.IsRevoked)
                .IsRequired();
        }
    }
}