namespace Bankampanya.Application.Features.Notifications.Dtos;

public sealed class NotificationListQuery
{
    public string? Search { get; init; }
    public string? Type { get; init; }
    public string? Status { get; init; }
}
