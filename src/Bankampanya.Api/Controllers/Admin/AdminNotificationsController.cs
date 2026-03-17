using Bankampanya.Application.Features.Notifications;
using Bankampanya.Application.Features.Notifications.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Bankampanya.Api.Controllers.Admin;

[ApiController]
[Route("api/admin/notifications")]
public class AdminNotificationsController(NotificationAdminService notificationAdminService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<NotificationAdminListItemDto>>> GetList(
        [FromQuery] string? search,
        [FromQuery] string? type,
        [FromQuery] string? status,
        CancellationToken cancellationToken)
    {
        var result = await notificationAdminService.GetListAsync(new NotificationListQuery
        {
            Search = search,
            Type = type,
            Status = status,
        }, cancellationToken);

        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<NotificationAdminListItemDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await notificationAdminService.GetByIdAsync(id, cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<NotificationAdminListItemDto>> Create(
        [FromBody] UpsertNotificationRequest request,
        CancellationToken cancellationToken)
    {
        var result = await notificationAdminService.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<NotificationAdminListItemDto>> Update(
        Guid id,
        [FromBody] UpsertNotificationRequest request,
        CancellationToken cancellationToken)
    {
        var result = await notificationAdminService.UpdateAsync(id, request, cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }
}
