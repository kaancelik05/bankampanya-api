using Bankampanya.Application.Features.MobileTracking;
using Bankampanya.Application.Features.MobileTracking.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Bankampanya.Api.Controllers.Mobile;

[ApiController]
[Route("api/mobile/tracking")]
public class MobileTrackingController(MobileTrackingQueryService mobileTrackingQueryService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<MobileTrackedCampaignDto>>> GetList(
        [FromQuery] string? search,
        [FromQuery] string? status,
        CancellationToken cancellationToken)
    {
        var result = await mobileTrackingQueryService.GetListAsync(new MobileTrackingListQuery
        {
            Search = search,
            Status = status,
        }, cancellationToken);

        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<MobileTrackedCampaignDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await mobileTrackingQueryService.GetByIdAsync(id, cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }
}
