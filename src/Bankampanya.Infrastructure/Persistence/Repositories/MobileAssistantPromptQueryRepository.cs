using Bankampanya.Application.Features.MobileAssistant;
using Bankampanya.Application.Features.MobileAssistant.Dtos;
using Bankampanya.Domain.Enums;
using Bankampanya.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Bankampanya.Infrastructure.Persistence.Repositories;

public sealed class MobileAssistantPromptQueryRepository(AppDbContext dbContext) : IMobileAssistantPromptQueryRepository
{
    public async Task<IReadOnlyList<MobileAssistantPromptSuggestionDto>> GetListAsync(
        MobileAssistantPromptListQuery query,
        CancellationToken cancellationToken)
    {
        var promptsQuery = dbContext.AssistantPromptTemplates
            .AsNoTracking()
            .Where(x => x.Status == PublishStatus.Live)
            .AsQueryable();

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
            .OrderByDescending(x => x.PublishedAtUtc ?? x.UpdatedAtUtc)
            .ToListAsync(cancellationToken);

        return prompts.Select(prompt => new MobileAssistantPromptSuggestionDto
        {
            Id = prompt.Id,
            Text = prompt.Text,
        }).ToList();
    }
}
