using Bankampanya.Domain.Enums;

namespace Bankampanya.Application.Features.Tracking.Dtos;

public sealed class UpsertTrackingRequest
{
    public Guid CampaignId { get; init; }
    public string BankName { get; init; } = string.Empty;
    public string Title { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string RequirementText { get; init; } = string.Empty;
    public string NextActionText { get; init; } = string.Empty;
    public string RewardText { get; init; } = string.Empty;
    public int? DefaultProgressTarget { get; init; }
    public TrackingProgressStatus ProgressStatus { get; init; }
    public PublishStatus Status { get; init; }
}
