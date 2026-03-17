using Bankampanya.Application.Features.Campaigns;
using Bankampanya.Application.Features.Campaigns.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Bankampanya.Api.Controllers.Admin;

[ApiController]
[Route("api/admin/campaigns")]
public class AdminCampaignsController(CampaignAdminService campaignAdminService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<CampaignAdminListItemDto>>> GetList(
        [FromQuery] string? search,
        [FromQuery] string? bankName,
        [FromQuery] string? category,
        [FromQuery] string? status,
        CancellationToken cancellationToken)
    {
        var result = await campaignAdminService.GetListAsync(new CampaignListQuery
        {
            Search = search,
            BankName = bankName,
            Category = category,
            Status = status,
        }, cancellationToken);

        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<CampaignAdminListItemDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await campaignAdminService.GetByIdAsync(id, cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<CampaignAdminListItemDto>> Create(
        [FromBody] UpsertCampaignRequest request,
        CancellationToken cancellationToken)
    {
        var result = await campaignAdminService.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<CampaignAdminListItemDto>> Update(
        Guid id,
        [FromBody] UpsertCampaignRequest request,
        CancellationToken cancellationToken)
    {
        var result = await campaignAdminService.UpdateAsync(id, request, cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }
}
