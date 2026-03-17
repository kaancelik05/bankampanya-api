using Bankampanya.Domain.Common;
using Bankampanya.Domain.Enums;

namespace Bankampanya.Domain.Entities;

public class TrackingTemplate : PublishableEntity
{
    public Guid CampaignId { get; set; }
    public string BankName { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string RequirementText { get; set; } = string.Empty;
    public string NextActionText { get; set; } = string.Empty;
    public string RewardText { get; set; } = string.Empty;
    public int? DefaultProgressTarget { get; set; }
    public TrackingProgressStatus ProgressStatus { get; set; }

    public Campaign? Campaign { get; set; }
}
