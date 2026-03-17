namespace Bankampanya.Application.Features.MobileEarnings.Dtos;

public sealed class MobileEarningsDashboardDto
{
    public MobileEarningsSummaryDto Summary { get; init; } = new();
    public IReadOnlyCollection<MobileEarningsStatDto> Stats { get; init; } = [];
    public IReadOnlyCollection<MobileEarningsHistoryItemDto> History { get; init; } = [];
    public IReadOnlyCollection<MobilePotentialEarningItemDto> Potential { get; init; } = [];
    public IReadOnlyCollection<MobileEarningsAlertDto> Alerts { get; init; } = [];
}
