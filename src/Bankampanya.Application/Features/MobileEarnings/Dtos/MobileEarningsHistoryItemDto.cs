namespace Bankampanya.Application.Features.MobileEarnings.Dtos;

public sealed class MobileEarningsHistoryItemDto
{
    public string Id { get; init; } = string.Empty;
    public string Title { get; init; } = string.Empty;
    public string BankName { get; init; } = string.Empty;
    public string RewardText { get; init; } = string.Empty;
    public string Status { get; init; } = string.Empty;
    public string DateLabel { get; init; } = string.Empty;
}
