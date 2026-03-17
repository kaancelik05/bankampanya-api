using Bankampanya.Domain.Enums;

namespace Bankampanya.Application.Features.Assistant.Dtos;

public sealed class AssistantPromptAdminListItemDto
{
    public Guid Id { get; init; }
    public string Text { get; init; } = string.Empty;
    public string Tone { get; init; } = string.Empty;
    public PublishStatus Status { get; init; }
    public DateTime CreatedAtUtc { get; init; }
    public DateTime UpdatedAtUtc { get; init; }
    public DateTime? PublishedAtUtc { get; init; }
}
