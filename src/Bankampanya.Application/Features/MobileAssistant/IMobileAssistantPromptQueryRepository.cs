using Bankampanya.Application.Features.MobileAssistant.Dtos;

namespace Bankampanya.Application.Features.MobileAssistant;

public interface IMobileAssistantPromptQueryRepository
{
    Task<IReadOnlyList<MobileAssistantPromptSuggestionDto>> GetListAsync(MobileAssistantPromptListQuery query, CancellationToken cancellationToken);
}
