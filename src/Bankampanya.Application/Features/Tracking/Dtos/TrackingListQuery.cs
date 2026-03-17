namespace Bankampanya.Application.Features.Tracking.Dtos;

public sealed class TrackingListQuery
{
    public string? Search { get; init; }
    public Guid? CampaignId { get; init; }
    public string? Status { get; init; }
    public string? ProgressStatus { get; init; }
}
