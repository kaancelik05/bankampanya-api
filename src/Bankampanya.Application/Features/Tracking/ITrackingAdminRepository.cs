using Bankampanya.Application.Features.Tracking.Dtos;

namespace Bankampanya.Application.Features.Tracking;

public interface ITrackingAdminRepository
{
    Task<IReadOnlyList<TrackingAdminListItemDto>> GetListAsync(TrackingListQuery query, CancellationToken cancellationToken);
    Task<TrackingAdminListItemDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<TrackingAdminListItemDto> CreateAsync(UpsertTrackingRequest request, CancellationToken cancellationToken);
    Task<TrackingAdminListItemDto?> UpdateAsync(Guid id, UpsertTrackingRequest request, CancellationToken cancellationToken);
}
