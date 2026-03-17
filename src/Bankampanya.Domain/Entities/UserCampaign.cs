using Bankampanya.Domain.Common;
using Bankampanya.Domain.Enums;

namespace Bankampanya.Domain.Entities;

public class UserCampaign : BaseEntity
{
    public Guid UserId { get; set; }
    public Guid CampaignId { get; set; }
    public Guid? TrackingTemplateId { get; set; }
    public UserCampaignStatus Status { get; set; }
    public int ProgressCurrent { get; set; }
    public int ProgressTarget { get; set; }
    public DateTime JoinedAtUtc { get; set; }
    public DateTime? CompletedAtUtc { get; set; }
    public DateTime? RewardedAtUtc { get; set; }

    public Campaign? Campaign { get; set; }
    public TrackingTemplate? TrackingTemplate { get; set; }
    public ICollection<TrackingEvent> TrackingEvents { get; set; } = new List<TrackingEvent>();
}
