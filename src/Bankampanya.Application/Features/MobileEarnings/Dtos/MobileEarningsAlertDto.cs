namespace Bankampanya.Application.Features.MobileEarnings.Dtos;

public sealed class MobileEarningsAlertDto
{
    public string Id { get; init; } = string.Empty;
    public string Title { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string Tone { get; init; } = string.Empty;
}
