using Bankampanya.Application.Features.MobileNotifications.Dtos;

namespace Bankampanya.Application.Features.MobileNotifications;

public interface IMobileNotificationQueryRepository
{
    Task<IReadOnlyList<MobileNotificationItemDto>> GetListAsync(MobileNotificationListQuery query, CancellationToken cancellationToken);
}
