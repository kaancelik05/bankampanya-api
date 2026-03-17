using Bankampanya.Application.Features.MobileProfile;
using Bankampanya.Application.Features.MobileProfile.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Bankampanya.Api.Controllers.Mobile;

[ApiController]
[Route("api/mobile/profile")]
public class MobileProfileController(MobileProfileQueryService mobileProfileQueryService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<MobileProfileDto>> Get(CancellationToken cancellationToken)
    {
        var result = await mobileProfileQueryService.GetAsync(cancellationToken);
        return Ok(result);
    }
}
