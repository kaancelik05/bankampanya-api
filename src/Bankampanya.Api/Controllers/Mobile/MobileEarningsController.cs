using Bankampanya.Application.Features.MobileEarnings;
using Bankampanya.Application.Features.MobileEarnings.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Bankampanya.Api.Controllers.Mobile;

[ApiController]
[Route("api/mobile/earnings")]
public class MobileEarningsController(MobileEarningsQueryService mobileEarningsQueryService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<MobileEarningsDashboardDto>> GetDashboard(CancellationToken cancellationToken)
    {
        var result = await mobileEarningsQueryService.GetDashboardAsync(cancellationToken);
        return Ok(result);
    }
}
