using Bankampanya.Domain.Common;

namespace Bankampanya.Domain.Entities;

public class TrackingEvent : BaseEntity
{
    public Guid UserCampaignId { get; set; }
    public DateTime OccurredAtUtc { get; set; }
    public string MerchantName { get; set; } = string.Empty;
    public decimal? Amount { get; set; }
    public string AmountText { get; set; } = string.Empty;
    public bool Qualified { get; set; }
    public string? Note { get; set; }

    public UserCampaign? UserCampaign { get; set; }
}
