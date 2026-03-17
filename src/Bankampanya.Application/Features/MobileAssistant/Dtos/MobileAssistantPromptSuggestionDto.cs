namespace Bankampanya.Application.Features.MobileAssistant.Dtos;

public sealed class MobileAssistantPromptSuggestionDto
{
    public Guid Id { get; init; }
    public string Text { get; init; } = string.Empty;
}
