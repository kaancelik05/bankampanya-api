using Bankampanya.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bankampanya.Infrastructure.Persistence.Configurations;

public class CampaignTermConfiguration : IEntityTypeConfiguration<CampaignTerm>
{
    public void Configure(EntityTypeBuilder<CampaignTerm> builder)
    {
        builder.ToTable("campaign_terms");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Text).HasMaxLength(500).IsRequired();
        builder.Property(x => x.SortOrder).IsRequired();
    }
}
