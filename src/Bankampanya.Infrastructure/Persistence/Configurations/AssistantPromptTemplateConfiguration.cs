using Bankampanya.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bankampanya.Infrastructure.Persistence.Configurations;

public class AssistantPromptTemplateConfiguration : IEntityTypeConfiguration<AssistantPromptTemplate>
{
    public void Configure(EntityTypeBuilder<AssistantPromptTemplate> builder)
    {
        builder.ToTable("assistant_prompt_templates");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Text).HasMaxLength(500).IsRequired();
        builder.Property(x => x.Tone).HasMaxLength(120).IsRequired();
        builder.Property(x => x.Status).IsRequired();
    }
}
