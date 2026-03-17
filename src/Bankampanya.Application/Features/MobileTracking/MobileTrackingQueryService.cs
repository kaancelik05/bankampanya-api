using Bankampanya.Application.Features.MobileTracking.Dtos;

namespace Bankampanya.Application.Features.MobileTracking;

public sealed class MobileTrackingQueryService(IMobileTrackingQueryRepository repository)
{
    public Task<IReadOnlyList<MobileTrackedCampaignDto>> GetListAsync(MobileTrackingListQuery query, CancellationToken cancellationToken)
        => repository.GetListAsync(query, cancellationToken);

    public Task<MobileTrackedCampaignDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        => repository.GetByIdAsync(id, cancellationToken);
}
