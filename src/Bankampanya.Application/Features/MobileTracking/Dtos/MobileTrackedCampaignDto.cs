using Bankampanya.Domain.Enums;

namespace Bankampanya.Application.Features.MobileTracking.Dtos;

public sealed class MobileTrackedCampaignDto
{
    public Guid Id { get; init; }
    public string Title { get; init; } = string.Empty;
    public string BankName { get; init; } = string.Empty;
    public int ProgressCurrent { get; init; }
    public int ProgressTarget { get; init; }
    public string DeadlineText { get; init; } = string.Empty;
    public string RewardText { get; init; } = string.Empty;
    public string ShortDescription { get; init; } = string.Empty;
    public string NextActionText { get; init; } = string.Empty;
    public TrackingProgressStatus Status { get; init; }
    public IReadOnlyCollection<string> Requirements { get; init; } = [];
    public IReadOnlyCollection<MobileTrackingEventDto> Events { get; init; } = [];
}
