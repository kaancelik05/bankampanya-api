using Bankampanya.Domain.Enums;

namespace Bankampanya.Application.Features.Assistant.Dtos;

public sealed class UpsertAssistantPromptRequest
{
    public string Text { get; init; } = string.Empty;
    public string Tone { get; init; } = string.Empty;
    public PublishStatus Status { get; init; }
}
