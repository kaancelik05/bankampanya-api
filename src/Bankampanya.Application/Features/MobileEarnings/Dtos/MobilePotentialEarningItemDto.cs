namespace Bankampanya.Application.Features.MobileEarnings.Dtos;

public sealed class MobilePotentialEarningItemDto
{
    public string Id { get; init; } = string.Empty;
    public string Title { get; init; } = string.Empty;
    public string BankName { get; init; } = string.Empty;
    public string PotentialText { get; init; } = string.Empty;
    public string RemainingActionText { get; init; } = string.Empty;
}
