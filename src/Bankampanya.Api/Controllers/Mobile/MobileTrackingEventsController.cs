using Bankampanya.Application.Features.MobileTrackingEvents;
using Bankampanya.Application.Features.MobileTrackingEvents.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Bankampanya.Api.Controllers.Mobile;

[ApiController]
[Route("api/mobile/tracking")]
public sealed class MobileTrackingEventsController(MobileTrackingEventMutationService mobileTrackingEventMutationService) : ControllerBase
{
    [HttpPost("{id:guid}/events")]
    public async Task<ActionResult<TrackingEventMutationResultDto>> CreateEvent(
        Guid id,
        [FromBody] CreateTrackingEventRequest request,
        CancellationToken cancellationToken)
    {
        var result = await mobileTrackingEventMutationService.CreateAsync(id, request, cancellationToken);
        return Ok(result);
    }
}
