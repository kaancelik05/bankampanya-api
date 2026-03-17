using Bankampanya.Application.Features.Notifications.Dtos;

namespace Bankampanya.Application.Features.Notifications;

public interface INotificationAdminRepository
{
    Task<IReadOnlyList<NotificationAdminListItemDto>> GetListAsync(NotificationListQuery query, CancellationToken cancellationToken);
    Task<NotificationAdminListItemDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<NotificationAdminListItemDto> CreateAsync(UpsertNotificationRequest request, CancellationToken cancellationToken);
    Task<NotificationAdminListItemDto?> UpdateAsync(Guid id, UpsertNotificationRequest request, CancellationToken cancellationToken);
}
