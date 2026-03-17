using Bankampanya.Application.Features.Dashboard;
using Bankampanya.Application.Features.Dashboard.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Bankampanya.Api.Controllers.Admin;

[ApiController]
[Route("api/admin/dashboard-summary")]
public class AdminDashboardController(AdminDashboardService adminDashboardService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<AdminDashboardSummaryDto>> GetSummary(CancellationToken cancellationToken)
    {
        var result = await adminDashboardService.GetSummaryAsync(cancellationToken);
        return Ok(result);
    }
}
