using Bankampanya.Application.Features.MobileTracking.Dtos;

namespace Bankampanya.Application.Features.MobileTracking;

public interface IMobileTrackingQueryRepository
{
    Task<IReadOnlyList<MobileTrackedCampaignDto>> GetListAsync(MobileTrackingListQuery query, CancellationToken cancellationToken);
    Task<MobileTrackedCampaignDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
}
