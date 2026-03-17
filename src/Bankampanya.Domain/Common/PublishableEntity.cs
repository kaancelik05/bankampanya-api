using Bankampanya.Domain.Enums;

namespace Bankampanya.Domain.Common;

public abstract class PublishableEntity : BaseEntity
{
    public PublishStatus Status { get; set; }
    public DateTime? PublishedAtUtc { get; set; }
}
