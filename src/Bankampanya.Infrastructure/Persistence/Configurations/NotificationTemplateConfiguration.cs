using Bankampanya.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bankampanya.Infrastructure.Persistence.Configurations;

public class NotificationTemplateConfiguration : IEntityTypeConfiguration<NotificationTemplate>
{
    public void Configure(EntityTypeBuilder<NotificationTemplate> builder)
    {
        builder.ToTable("notification_templates");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Type).IsRequired();
        builder.Property(x => x.Title).HasMaxLength(200).IsRequired();
        builder.Property(x => x.Body).HasMaxLength(600).IsRequired();
        builder.Property(x => x.CtaLabel).HasMaxLength(120).IsRequired();
        builder.Property(x => x.Route).HasMaxLength(300).IsRequired();
        builder.Property(x => x.Tone).IsRequired();
        builder.Property(x => x.Status).IsRequired();
    }
}
