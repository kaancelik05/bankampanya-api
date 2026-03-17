using Bankampanya.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bankampanya.Infrastructure.Persistence.Configurations;

public sealed class UserCampaignConfiguration : IEntityTypeConfiguration<UserCampaign>
{
    public void Configure(EntityTypeBuilder<UserCampaign> builder)
    {
        builder.ToTable("user_campaigns");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.UserId).IsRequired();
        builder.Property(x => x.CampaignId).IsRequired();
        builder.Property(x => x.Status).IsRequired();
        builder.Property(x => x.ProgressCurrent).IsRequired();
        builder.Property(x => x.ProgressTarget).IsRequired();
        builder.Property(x => x.JoinedAtUtc).IsRequired();

        builder.HasOne(x => x.Campaign)
            .WithMany()
            .HasForeignKey(x => x.CampaignId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.TrackingTemplate)
            .WithMany()
            .HasForeignKey(x => x.TrackingTemplateId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(x => x.TrackingEvents)
            .WithOne(x => x.UserCampaign)
            .HasForeignKey(x => x.UserCampaignId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => new { x.UserId, x.CampaignId }).IsUnique();
    }
}
