using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SprintManager.Domain.Entities;

namespace SprintManager.Infrastructure.Configurations
{
    public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.HasKey(r => r.Id);

            builder.Property(r => r.UserId)
               .IsRequired();

            builder.Property(r => r.Token)
                .IsRequired();

            builder.Property(r => r.Expires)
                .IsRequired();

            builder.Property(r => r.IsRevoked)
                .IsRequired();
        }
    }
}