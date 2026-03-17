namespace Bankampanya.Api.Controllers.Dev;

using Bankampanya.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/dev/diagnostics")]
public sealed class DevDiagnosticsController(AppDbContext dbContext, IWebHostEnvironment environment) : ControllerBase
{
    [HttpGet("user-campaigns")]
    public async Task<ActionResult<object>> GetUserCampaigns(CancellationToken cancellationToken)
    {
        if (!environment.IsDevelopment())
        {
            return NotFound();
        }

        var items = await dbContext.UserCampaigns
            .AsNoTracking()
            .Where(x => x.UserId == Guid.Parse("11111111-1111-1111-1111-111111111111"))
            .OrderByDescending(x => x.UpdatedAtUtc)
            .Select(x => new
            {
                x.Id,
                x.CampaignId,
                x.TrackingTemplateId,
                x.Status,
                x.ProgressCurrent,
                x.ProgressTarget,
                x.UpdatedAtUtc,
            })
            .ToListAsync(cancellationToken);

        return Ok(items);
    }
}
