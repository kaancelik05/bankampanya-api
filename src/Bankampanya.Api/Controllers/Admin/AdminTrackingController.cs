using Bankampanya.Application.Features.Tracking;
using Bankampanya.Application.Features.Tracking.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Bankampanya.Api.Controllers.Admin;

[ApiController]
[Route("api/admin/tracking")]
public class AdminTrackingController(TrackingAdminService trackingAdminService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<TrackingAdminListItemDto>>> GetList(
        [FromQuery] string? search,
        [FromQuery] Guid? campaignId,
        [FromQuery] string? status,
        [FromQuery] string? progressStatus,
        CancellationToken cancellationToken)
    {
        var result = await trackingAdminService.GetListAsync(new TrackingListQuery
        {
            Search = search,
            CampaignId = campaignId,
            Status = status,
            ProgressStatus = progressStatus,
        }, cancellationToken);

        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<TrackingAdminListItemDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await trackingAdminService.GetByIdAsync(id, cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<TrackingAdminListItemDto>> Create(
        [FromBody] UpsertTrackingRequest request,
        CancellationToken cancellationToken)
    {
        var result = await trackingAdminService.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<TrackingAdminListItemDto>> Update(
        Guid id,
        [FromBody] UpsertTrackingRequest request,
        CancellationToken cancellationToken)
    {
        var result = await trackingAdminService.UpdateAsync(id, request, cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }
}
