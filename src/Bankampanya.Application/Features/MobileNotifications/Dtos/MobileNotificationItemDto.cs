using Bankampanya.Domain.Enums;

namespace Bankampanya.Application.Features.MobileNotifications.Dtos;

public sealed class MobileNotificationItemDto
{
    public Guid Id { get; init; }
    public NotificationType Type { get; init; }
    public string Title { get; init; } = string.Empty;
    public string Body { get; init; } = string.Empty;
    public string TimeLabel { get; init; } = string.Empty;
    public string CtaLabel { get; init; } = string.Empty;
    public string Route { get; init; } = string.Empty;
    public string Tone { get; init; } = string.Empty;
}
