using Bankampanya.Application.Features.Assistant.Dtos;

namespace Bankampanya.Application.Features.Assistant;

public interface IAssistantPromptAdminRepository
{
    Task<IReadOnlyList<AssistantPromptAdminListItemDto>> GetListAsync(AssistantPromptListQuery query, CancellationToken cancellationToken);
    Task<AssistantPromptAdminListItemDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<AssistantPromptAdminListItemDto> CreateAsync(UpsertAssistantPromptRequest request, CancellationToken cancellationToken);
    Task<AssistantPromptAdminListItemDto?> UpdateAsync(Guid id, UpsertAssistantPromptRequest request, CancellationToken cancellationToken);
}
