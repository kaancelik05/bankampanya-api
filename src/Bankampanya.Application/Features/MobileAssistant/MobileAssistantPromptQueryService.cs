using Bankampanya.Application.Features.MobileAssistant.Dtos;

namespace Bankampanya.Application.Features.MobileAssistant;

public sealed class MobileAssistantPromptQueryService(IMobileAssistantPromptQueryRepository repository)
{
    public Task<IReadOnlyList<MobileAssistantPromptSuggestionDto>> GetListAsync(
        MobileAssistantPromptListQuery query,
        CancellationToken cancellationToken)
        => repository.GetListAsync(query, cancellationToken);
}
