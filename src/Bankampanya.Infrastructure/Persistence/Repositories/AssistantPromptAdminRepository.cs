using Bankampanya.Application.Features.Assistant;
using Bankampanya.Application.Features.Assistant.Dtos;
using Bankampanya.Domain.Entities;
using Bankampanya.Domain.Enums;
using Bankampanya.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Bankampanya.Infrastructure.Persistence.Repositories;

public sealed class AssistantPromptAdminRepository(AppDbContext dbContext) : IAssistantPromptAdminRepository
{
    public async Task<IReadOnlyList<AssistantPromptAdminListItemDto>> GetListAsync(AssistantPromptListQuery query, CancellationToken cancellationToken)
    {
        var promptsQuery = dbContext.AssistantPromptTemplates
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.Status) && Enum.TryParse<PublishStatus>(query.Status, true, out var status))
        {
            promptsQuery = promptsQuery.Where(x => x.Status == status);
        }

        if (!string.IsNullOrWhiteSpace(query.Tone))
        {
            var tone = query.Tone.Trim();
            promptsQuery = promptsQuery.Where(x => x.Tone.Contains(tone));
        }

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var search = query.Search.Trim();
            promptsQuery = promptsQuery.Where(x => x.Text.Contains(search) || x.Tone.Contains(search));
        }

        var prompts = await promptsQuery
            .OrderByDescending(x => x.UpdatedAtUtc)
            .ToListAsync(cancellationToken);

        return prompts.Select(MapToDto).ToList();
    }

    public async Task<AssistantPromptAdminListItemDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var prompt = await dbContext.AssistantPromptTemplates
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        return prompt is null ? null : MapToDto(prompt);
    }

    public async Task<AssistantPromptAdminListItemDto> CreateAsync(UpsertAssistantPromptRequest request, CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;
        var prompt = new AssistantPromptTemplate
        {
            Id = Guid.NewGuid(),
            Text = request.Text.Trim(),
            Tone = request.Tone.Trim(),
            Status = request.Status,
            CreatedAtUtc = now,
            UpdatedAtUtc = now,
            PublishedAtUtc = request.Status == PublishStatus.Live ? now : null,
        };

        dbContext.AssistantPromptTemplates.Add(prompt);
        await dbContext.SaveChangesAsync(cancellationToken);

        return MapToDto(prompt);
    }

    public async Task<AssistantPromptAdminListItemDto?> UpdateAsync(Guid id, UpsertAssistantPromptRequest request, CancellationToken cancellationToken)
    {
        var prompt = await dbContext.AssistantPromptTemplates
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (prompt is null)
        {
            return null;
        }

        var now = DateTime.UtcNow;
        prompt.Text = request.Text.Trim();
        prompt.Tone = request.Tone.Trim();
        prompt.Status = request.Status;
        prompt.UpdatedAtUtc = now;
        prompt.PublishedAtUtc ??= request.Status == PublishStatus.Live ? now : null;

        await dbContext.SaveChangesAsync(cancellationToken);

        return MapToDto(prompt);
    }

    private static AssistantPromptAdminListItemDto MapToDto(AssistantPromptTemplate prompt)
    {
        return new AssistantPromptAdminListItemDto
        {
            Id = prompt.Id,
            Text = prompt.Text,
            Tone = prompt.Tone,
            Status = prompt.Status,
            CreatedAtUtc = prompt.CreatedAtUtc,
            UpdatedAtUtc = prompt.UpdatedAtUtc,
            PublishedAtUtc = prompt.PublishedAtUtc,
        };
    }
}
