using Bankampanya.Application.Features.Notifications;
using Bankampanya.Application.Features.Notifications.Dtos;
using Bankampanya.Domain.Entities;
using Bankampanya.Domain.Enums;
using Bankampanya.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Bankampanya.Infrastructure.Persistence.Repositories;

public sealed class NotificationAdminRepository(AppDbContext dbContext) : INotificationAdminRepository
{
    public async Task<IReadOnlyList<NotificationAdminListItemDto>> GetListAsync(NotificationListQuery query, CancellationToken cancellationToken)
    {
        var notificationsQuery = dbContext.NotificationTemplates
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.Status) && Enum.TryParse<PublishStatus>(query.Status, true, out var status))
        {
            notificationsQuery = notificationsQuery.Where(x => x.Status == status);
        }

        if (!string.IsNullOrWhiteSpace(query.Type) && Enum.TryParse<NotificationType>(query.Type, true, out var type))
        {
            notificationsQuery = notificationsQuery.Where(x => x.Type == type);
        }

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var search = query.Search.Trim();
            notificationsQuery = notificationsQuery.Where(x =>
                x.Title.Contains(search) ||
                x.Body.Contains(search) ||
                x.Route.Contains(search));
        }

        var notifications = await notificationsQuery
            .OrderByDescending(x => x.UpdatedAtUtc)
            .ToListAsync(cancellationToken);

        return notifications.Select(MapToDto).ToList();
    }

    public async Task<NotificationAdminListItemDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var notification = await dbContext.NotificationTemplates
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        return notification is null ? null : MapToDto(notification);
    }

    public async Task<NotificationAdminListItemDto> CreateAsync(UpsertNotificationRequest request, CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;
        var notification = new NotificationTemplate
        {
            Id = Guid.NewGuid(),
            Type = request.Type,
            Title = request.Title.Trim(),
            Body = request.Body.Trim(),
            CtaLabel = request.CtaLabel.Trim(),
            Route = request.Route.Trim(),
            Tone = request.Tone,
            Status = request.Status,
            CreatedAtUtc = now,
            UpdatedAtUtc = now,
            PublishedAtUtc = request.Status == PublishStatus.Live ? now : null,
        };

        dbContext.NotificationTemplates.Add(notification);
        await dbContext.SaveChangesAsync(cancellationToken);

        return MapToDto(notification);
    }

    public async Task<NotificationAdminListItemDto?> UpdateAsync(Guid id, UpsertNotificationRequest request, CancellationToken cancellationToken)
    {
        var notification = await dbContext.NotificationTemplates
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (notification is null)
        {
            return null;
        }

        var now = DateTime.UtcNow;
        notification.Type = request.Type;
        notification.Title = request.Title.Trim();
        notification.Body = request.Body.Trim();
        notification.CtaLabel = request.CtaLabel.Trim();
        notification.Route = request.Route.Trim();
        notification.Tone = request.Tone;
        notification.Status = request.Status;
        notification.UpdatedAtUtc = now;
        notification.PublishedAtUtc ??= request.Status == PublishStatus.Live ? now : null;

        await dbContext.SaveChangesAsync(cancellationToken);

        return MapToDto(notification);
    }

    private static NotificationAdminListItemDto MapToDto(NotificationTemplate notification)
    {
        return new NotificationAdminListItemDto
        {
            Id = notification.Id,
            Type = notification.Type,
            Title = notification.Title,
            Body = notification.Body,
            CtaLabel = notification.CtaLabel,
            Route = notification.Route,
            Tone = notification.Tone,
            Status = notification.Status,
            CreatedAtUtc = notification.CreatedAtUtc,
            UpdatedAtUtc = notification.UpdatedAtUtc,
            PublishedAtUtc = notification.PublishedAtUtc,
        };
    }
}
