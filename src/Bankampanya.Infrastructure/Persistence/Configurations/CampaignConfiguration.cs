using Bankampanya.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bankampanya.Infrastructure.Persistence.Configurations;

public class CampaignConfiguration : IEntityTypeConfiguration<Campaign>
{
    public void Configure(EntityTypeBuilder<Campaign> builder)
    {
        builder.ToTable("campaigns");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.BankName).HasMaxLength(120).IsRequired();
        builder.Property(x => x.Category).HasMaxLength(120).IsRequired();
        builder.Property(x => x.Title).HasMaxLength(200).IsRequired();
        builder.Property(x => x.ShortDescription).HasMaxLength(500).IsRequired();
        builder.Property(x => x.RewardText).HasMaxLength(120).IsRequired();
        builder.Property(x => x.RewardType).IsRequired();
        builder.Property(x => x.DeadlineText).HasMaxLength(120).IsRequired();
        builder.Property(x => x.ValidDateRangeLabel).HasMaxLength(120).IsRequired();
        builder.Property(x => x.NextActionText).HasMaxLength(500);
        builder.Property(x => x.Status).IsRequired();

        builder.HasMany(x => x.Terms)
            .WithOne(x => x.Campaign)
            .HasForeignKey(x => x.CampaignId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
