using Bankampanya.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bankampanya.Infrastructure.Persistence.Configurations;

public sealed class TrackingEventConfiguration : IEntityTypeConfiguration<TrackingEvent>
{
    public void Configure(EntityTypeBuilder<TrackingEvent> builder)
    {
        builder.ToTable("tracking_events");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.UserCampaignId).IsRequired();
        builder.Property(x => x.OccurredAtUtc).IsRequired();
        builder.Property(x => x.MerchantName).HasMaxLength(160).IsRequired();
        builder.Property(x => x.Amount).HasPrecision(18, 2);
        builder.Property(x => x.AmountText).HasMaxLength(120).IsRequired();
        builder.Property(x => x.Qualified).IsRequired();
        builder.Property(x => x.Note).HasMaxLength(500);

        builder.HasIndex(x => new { x.UserCampaignId, x.OccurredAtUtc });
    }
}
