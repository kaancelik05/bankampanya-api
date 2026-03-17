using Bankampanya.Application.Features.MobileCredits;
using Bankampanya.Application.Features.MobileCredits.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Bankampanya.Api.Controllers.Mobile;

[ApiController]
[Route("api/mobile/credits")]
public class MobileCreditsController(MobileCreditQueryService mobileCreditQueryService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<MobileCreditListItemDto>>> GetList(
        [FromQuery] string? search,
        [FromQuery] string? type,
        CancellationToken cancellationToken)
    {
        var result = await mobileCreditQueryService.GetListAsync(new MobileCreditListQuery
        {
            Search = search,
            Type = type,
        }, cancellationToken);

        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<MobileCreditDetailDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await mobileCreditQueryService.GetByIdAsync(id, cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }
}
