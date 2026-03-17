using Bankampanya.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bankampanya.Infrastructure.Persistence.Configurations;

public sealed class WalletCardConfiguration : IEntityTypeConfiguration<WalletCard>
{
    public void Configure(EntityTypeBuilder<WalletCard> builder)
    {
        builder.ToTable("wallet_cards");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.UserId).IsRequired();
        builder.Property(x => x.BankName).HasMaxLength(120).IsRequired();
        builder.Property(x => x.CardType).HasMaxLength(120).IsRequired();
        builder.Property(x => x.CustomName).HasMaxLength(160).IsRequired();
        builder.Property(x => x.Status).IsRequired();

        builder.HasIndex(x => new { x.UserId, x.BankName });
    }
}
