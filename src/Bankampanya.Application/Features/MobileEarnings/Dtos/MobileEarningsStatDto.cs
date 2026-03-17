namespace Bankampanya.Application.Features.MobileEarnings.Dtos;

public sealed class MobileEarningsStatDto
{
    public string Id { get; init; } = string.Empty;
    public string Label { get; init; } = string.Empty;
    public string ValueText { get; init; } = string.Empty;
    public string Tone { get; init; } = "default";
}
