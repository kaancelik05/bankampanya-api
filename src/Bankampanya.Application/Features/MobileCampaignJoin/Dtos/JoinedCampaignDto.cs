namespace Bankampanya.Application.Features.MobileCampaignJoin.Dtos;

public sealed class JoinedCampaignDto
{
    public Guid Id { get; init; }
    public Guid CampaignId { get; init; }
    public Guid? TrackingTemplateId { get; init; }
    public string Status { get; init; } = string.Empty;
    public int ProgressCurrent { get; init; }
    public int ProgressTarget { get; init; }
    public string JoinedAtLabel { get; init; } = string.Empty;
}
