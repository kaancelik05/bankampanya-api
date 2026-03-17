using Bankampanya.Domain.Common;

namespace Bankampanya.Domain.Entities;

public class CampaignTerm : BaseEntity
{
    public Guid CampaignId { get; set; }
    public int SortOrder { get; set; }
    public string Text { get; set; } = string.Empty;

    public Campaign? Campaign { get; set; }
}
