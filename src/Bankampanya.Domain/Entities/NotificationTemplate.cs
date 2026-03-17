using Bankampanya.Domain.Common;
using Bankampanya.Domain.Enums;

namespace Bankampanya.Domain.Entities;

public class NotificationTemplate : PublishableEntity
{
    public NotificationType Type { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public string CtaLabel { get; set; } = string.Empty;
    public string Route { get; set; } = string.Empty;
    public NotificationTone Tone { get; set; }
}
