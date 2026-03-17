using Bankampanya.Application.Features.MobileNotifications;
using Bankampanya.Application.Features.MobileNotifications.Dtos;
using Bankampanya.Domain.Entities;
using Bankampanya.Domain.Enums;
using Bankampanya.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Bankampanya.Infrastructure.Persistence.Repositories;

public sealed class MobileNotificationQueryRepository(AppDbContext dbContext) : IMobileNotificationQueryRepository
{
    public async Task<IReadOnlyList<MobileNotificationItemDto>> GetListAsync(MobileNotificationListQuery query, CancellationToken cancellationToken)
    {
        var notificationsQuery = dbContext.NotificationTemplates
            .AsNoTracking()
            .Where(x => x.Status == PublishStatus.Live)
            .AsQueryable();

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
            .OrderByDescending(x => x.PublishedAtUtc ?? x.UpdatedAtUtc)
            .ToListAsync(cancellationToken);

        return notifications.Select(MapItem).ToList();
    }

    private static MobileNotificationItemDto MapItem(NotificationTemplate notification)
    {
        return new MobileNotificationItemDto
        {
            Id = notification.Id,
            Type = notification.Type,
            Title = notification.Title,
            Body = notification.Body,
            TimeLabel = BuildTimeLabel(notification),
            CtaLabel = notification.CtaLabel,
            Route = notification.Route,
            Tone = MapTone(notification.Tone),
        };
    }

    private static string BuildTimeLabel(NotificationTemplate notification)
    {
        var timestamp = notification.PublishedAtUtc ?? notification.UpdatedAtUtc;
        var diff = DateTime.UtcNow - timestamp;

        if (diff.TotalHours < 1)
        {
            return "Az önce";
        }

        if (diff.TotalDays < 1)
        {
            return $"{Math.Max(1, (int)diff.TotalHours)} sa önce";
        }

        return $"{Math.Max(1, (int)diff.TotalDays)} gün önce";
    }

    private static string MapTone(NotificationTone tone)
    {
        return tone switch
        {
            NotificationTone.Success => "success",
            NotificationTone.Warning => "warning",
            NotificationTone.Info => "info",
            NotificationTone.Danger => "danger",
            _ => "info",
        };
    }
}
