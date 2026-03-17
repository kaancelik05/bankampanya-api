using Bankampanya.Application.Features.MobileNotifications.Dtos;

namespace Bankampanya.Application.Features.MobileNotifications;

public sealed class MobileNotificationQueryService(IMobileNotificationQueryRepository repository)
{
    public Task<IReadOnlyList<MobileNotificationItemDto>> GetListAsync(MobileNotificationListQuery query, CancellationToken cancellationToken)
        => repository.GetListAsync(query, cancellationToken);
}
