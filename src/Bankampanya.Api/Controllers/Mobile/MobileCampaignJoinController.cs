using Bankampanya.Application.Features.MobileCampaignJoin;
using Bankampanya.Application.Features.MobileCampaignJoin.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Bankampanya.Api.Controllers.Mobile;

[ApiController]
[Route("api/mobile/campaigns")]
public sealed class MobileCampaignJoinController(MobileCampaignJoinService mobileCampaignJoinService) : ControllerBase
{
    [HttpPost("{id:guid}/join")]
    public async Task<ActionResult<JoinedCampaignDto>> Join(
        Guid id,
        [FromBody] JoinCampaignRequest request,
        CancellationToken cancellationToken)
    {
        var result = await mobileCampaignJoinService.JoinAsync(id, cancellationToken);
        return Ok(result);
    }
}
