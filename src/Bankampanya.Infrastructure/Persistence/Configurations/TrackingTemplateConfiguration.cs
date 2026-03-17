using Bankampanya.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bankampanya.Infrastructure.Persistence.Configurations;

public class TrackingTemplateConfiguration : IEntityTypeConfiguration<TrackingTemplate>
{
    public void Configure(EntityTypeBuilder<TrackingTemplate> builder)
    {
        builder.ToTable("tracking_templates");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.BankName).HasMaxLength(120).IsRequired();
        builder.Property(x => x.Title).HasMaxLength(200).IsRequired();
        builder.Property(x => x.Description).HasMaxLength(500).IsRequired();
        builder.Property(x => x.RequirementText).HasMaxLength(500).IsRequired();
        builder.Property(x => x.NextActionText).HasMaxLength(500).IsRequired();
        builder.Property(x => x.RewardText).HasMaxLength(120).IsRequired();
        builder.Property(x => x.ProgressStatus).IsRequired();
        builder.Property(x => x.Status).IsRequired();

        builder.HasOne(x => x.Campaign)
            .WithMany(x => x.TrackingTemplates)
            .HasForeignKey(x => x.CampaignId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
