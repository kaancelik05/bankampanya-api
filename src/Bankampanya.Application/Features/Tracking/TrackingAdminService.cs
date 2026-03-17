using Bankampanya.Application.Features.Tracking.Dtos;

namespace Bankampanya.Application.Features.Tracking;

public sealed class TrackingAdminService(ITrackingAdminRepository repository)
{
    public Task<IReadOnlyList<TrackingAdminListItemDto>> GetListAsync(TrackingListQuery query, CancellationToken cancellationToken)
        => repository.GetListAsync(query, cancellationToken);

    public Task<TrackingAdminListItemDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        => repository.GetByIdAsync(id, cancellationToken);

    public Task<TrackingAdminListItemDto> CreateAsync(UpsertTrackingRequest request, CancellationToken cancellationToken)
        => repository.CreateAsync(request, cancellationToken);

    public Task<TrackingAdminListItemDto?> UpdateAsync(Guid id, UpsertTrackingRequest request, CancellationToken cancellationToken)
        => repository.UpdateAsync(id, request, cancellationToken);
}
