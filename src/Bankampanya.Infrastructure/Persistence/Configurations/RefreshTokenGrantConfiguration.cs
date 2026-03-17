using Bankampanya.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bankampanya.Infrastructure.Persistence.Configurations;

public sealed class RefreshTokenGrantConfiguration : IEntityTypeConfiguration<RefreshTokenGrant>
{
    public void Configure(EntityTypeBuilder<RefreshTokenGrant> builder)
    {
        builder.ToTable("refresh_token_grants");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.TokenHash).HasMaxLength(256).IsRequired();

        builder.HasIndex(x => x.TokenHash).IsUnique();
        builder.HasIndex(x => new { x.UserId, x.RevokedAtUtc });

        builder.HasOne(x => x.User)
            .WithMany(x => x.RefreshTokens)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
