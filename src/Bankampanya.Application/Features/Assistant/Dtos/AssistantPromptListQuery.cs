namespace Bankampanya.Application.Features.Assistant.Dtos;

public sealed class AssistantPromptListQuery
{
    public string? Search { get; init; }
    public string? Tone { get; init; }
    public string? Status { get; init; }
}
