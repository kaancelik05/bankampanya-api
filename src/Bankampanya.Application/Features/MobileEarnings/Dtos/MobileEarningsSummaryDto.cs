namespace Bankampanya.Application.Features.MobileEarnings.Dtos;

public sealed class MobileEarningsSummaryDto
{
    public string MonthLabel { get; init; } = string.Empty;
    public string TotalEarnedText { get; init; } = string.Empty;
    public string PendingRewardText { get; init; } = string.Empty;
    public string PotentialRewardText { get; init; } = string.Empty;
}
