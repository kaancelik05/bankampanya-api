using Bankampanya.Domain.Common;
using Bankampanya.Domain.Enums;

namespace Bankampanya.Domain.Entities;

public class Campaign : PublishableEntity
{
    public string BankName { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string ShortDescription { get; set; } = string.Empty;
    public string RewardText { get; set; } = string.Empty;
    public RewardType RewardType { get; set; }
    public string DeadlineText { get; set; } = string.Empty;
    public DateTime? ValidFromUtc { get; set; }
    public DateTime? ValidToUtc { get; set; }
    public string ValidDateRangeLabel { get; set; } = string.Empty;
    public bool IsProgressive { get; set; }
    public int? ProgressTarget { get; set; }
    public string? NextActionText { get; set; }

    public ICollection<CampaignTerm> Terms { get; set; } = new List<CampaignTerm>();
    public ICollection<TrackingTemplate> TrackingTemplates { get; set; } = new List<TrackingTemplate>();
}
