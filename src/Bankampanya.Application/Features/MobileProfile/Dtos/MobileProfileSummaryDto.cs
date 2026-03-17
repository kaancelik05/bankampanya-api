namespace Bankampanya.Application.Features.MobileProfile.Dtos;

public sealed class MobileProfileSummaryDto
{
    public string FullName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string Phone { get; init; } = string.Empty;
    public string JoinedLabel { get; init; } = string.Empty;
    public int TotalCards { get; init; }
    public int ActiveTrackingCount { get; init; }
    public string MonthlyPotentialText { get; init; } = string.Empty;
}
