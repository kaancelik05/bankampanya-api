using Bankampanya.Application.Features.MobileTrackingEvents.Dtos;

namespace Bankampanya.Application.Features.MobileTrackingEvents;

public interface IMobileTrackingEventMutationRepository
{
    Task<TrackingEventMutationResultDto> CreateAsync(Guid trackingTemplateId, CreateTrackingEventRequest request, CancellationToken cancellationToken);
}
