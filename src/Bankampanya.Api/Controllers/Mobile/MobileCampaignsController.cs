using Bankampanya.Application.Features.MobileCampaigns;
using Bankampanya.Application.Features.MobileCampaigns.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Bankampanya.Api.Controllers.Mobile;

[ApiController]
[Route("api/mobile/campaigns")]
public class MobileCampaignsController(MobileCampaignQueryService mobileCampaignQueryService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<MobileCampaignListItemDto>>> GetList(
        [FromQuery] string? search,
        [FromQuery] string? category,
        [FromQuery] string? bankName,
        CancellationToken cancellationToken)
    {
        var result = await mobileCampaignQueryService.GetListAsync(new MobileCampaignListQuery
        {
            Search = search,
            Category = category,
            BankName = bankName,
        }, cancellationToken);

        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<MobileCampaignDetailDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await mobileCampaignQueryService.GetByIdAsync(id, cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }
}
