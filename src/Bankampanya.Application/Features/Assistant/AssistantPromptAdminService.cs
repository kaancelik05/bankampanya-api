using Bankampanya.Application.Features.Assistant.Dtos;

namespace Bankampanya.Application.Features.Assistant;

public sealed class AssistantPromptAdminService(IAssistantPromptAdminRepository repository)
{
    public Task<IReadOnlyList<AssistantPromptAdminListItemDto>> GetListAsync(AssistantPromptListQuery query, CancellationToken cancellationToken)
        => repository.GetListAsync(query, cancellationToken);

    public Task<AssistantPromptAdminListItemDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        => repository.GetByIdAsync(id, cancellationToken);

    public Task<AssistantPromptAdminListItemDto> CreateAsync(UpsertAssistantPromptRequest request, CancellationToken cancellationToken)
        => repository.CreateAsync(request, cancellationToken);

    public Task<AssistantPromptAdminListItemDto?> UpdateAsync(Guid id, UpsertAssistantPromptRequest request, CancellationToken cancellationToken)
        => repository.UpdateAsync(id, request, cancellationToken);
}
