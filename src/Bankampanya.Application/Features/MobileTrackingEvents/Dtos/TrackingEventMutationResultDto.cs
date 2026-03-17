namespace Bankampanya.Application.Features.MobileTrackingEvents.Dtos;

public sealed class TrackingEventMutationResultDto
{
    public Guid EventId { get; init; }
    public Guid UserCampaignId { get; init; }
    public int ProgressCurrent { get; init; }
    public int ProgressTarget { get; init; }
    public bool Qualified { get; init; }
    public string Status { get; init; } = string.Empty;
}
