using Bankampanya.Application.Features.MobileNotifications;
using Bankampanya.Application.Features.MobileNotifications.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Bankampanya.Api.Controllers.Mobile;

[ApiController]
[Route("api/mobile/notifications")]
public class MobileNotificationsController(MobileNotificationQueryService mobileNotificationQueryService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<MobileNotificationItemDto>>> GetList(
        [FromQuery] string? search,
        [FromQuery] string? type,
        CancellationToken cancellationToken)
    {
        var result = await mobileNotificationQueryService.GetListAsync(new MobileNotificationListQuery
        {
            Search = search,
            Type = type,
        }, cancellationToken);

        return Ok(result);
    }
}
