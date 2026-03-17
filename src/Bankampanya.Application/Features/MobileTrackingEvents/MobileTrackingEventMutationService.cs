using Bankampanya.Application.Features.MobileTrackingEvents.Dtos;

namespace Bankampanya.Application.Features.MobileTrackingEvents;

public sealed class MobileTrackingEventMutationService(IMobileTrackingEventMutationRepository repository)
{
    public Task<TrackingEventMutationResultDto> CreateAsync(
        Guid trackingTemplateId,
        CreateTrackingEventRequest request,
        CancellationToken cancellationToken)
        => repository.CreateAsync(trackingTemplateId, request, cancellationToken);
}
