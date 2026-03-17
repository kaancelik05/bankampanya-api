using Bankampanya.Domain.Enums;

namespace Bankampanya.Application.Features.Notifications.Dtos;

public sealed class NotificationAdminListItemDto
{
    public Guid Id { get; init; }
    public NotificationType Type { get; init; }
    public string Title { get; init; } = string.Empty;
    public string Body { get; init; } = string.Empty;
    public string CtaLabel { get; init; } = string.Empty;
    public string Route { get; init; } = string.Empty;
    public NotificationTone Tone { get; init; }
    public PublishStatus Status { get; init; }
    public DateTime CreatedAtUtc { get; init; }
    public DateTime UpdatedAtUtc { get; init; }
    public DateTime? PublishedAtUtc { get; init; }
}
