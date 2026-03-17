using Bankampanya.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bankampanya.Infrastructure.Persistence.Configurations;

public class CreditOfferConfiguration : IEntityTypeConfiguration<CreditOffer>
{
    public void Configure(EntityTypeBuilder<CreditOffer> builder)
    {
        builder.ToTable("credit_offers");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.BankName).HasMaxLength(120).IsRequired();
        builder.Property(x => x.Title).HasMaxLength(200).IsRequired();
        builder.Property(x => x.Type).IsRequired();
        builder.Property(x => x.Subtype);
        builder.Property(x => x.Rate).HasMaxLength(120).IsRequired();
        builder.Property(x => x.AmountRange).HasMaxLength(120).IsRequired();
        builder.Property(x => x.DetailSummary).HasMaxLength(600).IsRequired();
        builder.Property(x => x.Terms).HasColumnType("jsonb").IsRequired();
        builder.Property(x => x.Status).IsRequired();
    }
}
