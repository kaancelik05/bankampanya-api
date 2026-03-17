using Bankampanya.Application.Features.Notifications.Dtos;

namespace Bankampanya.Application.Features.Notifications;

public sealed class NotificationAdminService(INotificationAdminRepository repository)
{
    public Task<IReadOnlyList<NotificationAdminListItemDto>> GetListAsync(NotificationListQuery query, CancellationToken cancellationToken)
        => repository.GetListAsync(query, cancellationToken);

    public Task<NotificationAdminListItemDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        => repository.GetByIdAsync(id, cancellationToken);

    public Task<NotificationAdminListItemDto> CreateAsync(UpsertNotificationRequest request, CancellationToken cancellationToken)
        => repository.CreateAsync(request, cancellationToken);

    public Task<NotificationAdminListItemDto?> UpdateAsync(Guid id, UpsertNotificationRequest request, CancellationToken cancellationToken)
        => repository.UpdateAsync(id, request, cancellationToken);
}
