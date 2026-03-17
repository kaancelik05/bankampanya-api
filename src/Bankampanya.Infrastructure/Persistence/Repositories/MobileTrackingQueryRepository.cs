using Bankampanya.Application.Common.Interfaces;
using Bankampanya.Application.Features.MobileTracking;
using Bankampanya.Application.Features.MobileTracking.Dtos;
using Bankampanya.Domain.Enums;
using Bankampanya.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Bankampanya.Infrastructure.Persistence.Repositories;

public sealed class MobileTrackingQueryRepository(
    AppDbContext dbContext,
    ICurrentUserService currentUserService) : IMobileTrackingQueryRepository
{
    public async Task<IReadOnlyList<MobileTrackedCampaignDto>> GetListAsync(MobileTrackingListQuery query, CancellationToken cancellationToken)
    {
        var userId = currentUserService.GetUserId();

        var trackingRows = await dbContext.UserCampaigns
            .AsNoTracking()
            .Where(x => x.UserId == userId)
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
            .OrderByDescending(x => x.UpdatedAtUtc)
            .ToListAsync(cancellationToken);

        if (trackingRows.Count == 0)
        {
            return [];
        }

        if (!string.IsNullOrWhiteSpace(query.Status) && Enum.TryParse<UserCampaignStatus>(query.Status, true, out var userStatus))
        {
            trackingRows = trackingRows.Where(x => x.Status == userStatus).ToList();
        }

        var campaignIds = trackingRows.Select(x => x.CampaignId).Distinct().ToList();
        var trackingTemplateIds = trackingRows.Where(x => x.TrackingTemplateId.HasValue).Select(x => x.TrackingTemplateId!.Value).Distinct().ToList();
        var userCampaignIds = trackingRows.Select(x => x.Id).ToList();

        var campaigns = await dbContext.Campaigns
            .AsNoTracking()
            .Where(x => campaignIds.Contains(x.Id))
            .ToDictionaryAsync(x => x.Id, cancellationToken);

        var trackingTemplates = trackingTemplateIds.Count == 0
            ? new Dictionary<Guid, Domain.Entities.TrackingTemplate>()
            : await dbContext.TrackingTemplates
                .AsNoTracking()
                .Where(x => trackingTemplateIds.Contains(x.Id))
                .ToDictionaryAsync(x => x.Id, cancellationToken);

        var trackingEvents = await dbContext.TrackingEvents
            .AsNoTracking()
            .Where(x => userCampaignIds.Contains(x.UserCampaignId))
            .OrderByDescending(x => x.OccurredAtUtc)
            .Select(x => new
            {
                x.Id,
                x.UserCampaignId,
                x.OccurredAtUtc,
                x.AmountText,
                x.MerchantName,
                x.Qualified,
            })
            .ToListAsync(cancellationToken);

        var eventsByCampaign = trackingEvents
            .GroupBy(x => x.UserCampaignId)
            .ToDictionary(
                group => group.Key,
                group => (IReadOnlyCollection<MobileTrackingEventDto>)group
                    .Select(x => new MobileTrackingEventDto
                    {
                        Id = x.Id.ToString(),
                        DateLabel = x.OccurredAtUtc.Date == DateTime.UtcNow.Date ? "Bugün" : x.OccurredAtUtc.ToString("dd MMM"),
                        AmountText = x.AmountText,
                        MerchantName = x.MerchantName,
                        Qualified = x.Qualified,
                    })
                    .ToArray());

        var items = trackingRows.Select(row =>
        {
            campaigns.TryGetValue(row.CampaignId, out var campaign);
            var tracking = row.TrackingTemplateId.HasValue && trackingTemplates.TryGetValue(row.TrackingTemplateId.Value, out var foundTracking)
                ? foundTracking
                : null;

            return new MobileTrackedCampaignDto
            {
                Id = row.TrackingTemplateId ?? row.Id,
                Title = tracking?.Title ?? campaign?.Title ?? "Takip Kampanyası",
                BankName = tracking?.BankName ?? campaign?.BankName ?? string.Empty,
                ProgressCurrent = row.ProgressCurrent,
                ProgressTarget = row.ProgressTarget,
                DeadlineText = campaign?.DeadlineText ?? "Yakında sona eriyor",
                RewardText = tracking?.RewardText ?? campaign?.RewardText ?? string.Empty,
                ShortDescription = tracking?.Description ?? campaign?.ShortDescription ?? string.Empty,
                NextActionText = tracking?.NextActionText ?? campaign?.NextActionText ?? string.Empty,
                Status = MapStatus(row.Status),
                Requirements = BuildRequirements(tracking?.RequirementText, row.ProgressCurrent, row.ProgressTarget),
                Events = eventsByCampaign.TryGetValue(row.Id, out var events) ? events : [],
            };
        }).ToList();

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var search = query.Search.Trim();
            items = items.Where(x =>
                x.Title.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                x.BankName.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                x.ShortDescription.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                x.Requirements.Any(req => req.Contains(search, StringComparison.OrdinalIgnoreCase)))
                .ToList();
        }

        return items;
    }

    public async Task<MobileTrackedCampaignDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var userId = currentUserService.GetUserId();

        var row = await dbContext.UserCampaigns
            .AsNoTracking()
            .Where(x => x.UserId == userId && (x.TrackingTemplateId == id || x.CampaignId == id))
            .OrderByDescending(x => x.UpdatedAtUtc)
            .Select(x => new
            {
                x.Id,
                x.CampaignId,
                x.TrackingTemplateId,
                x.Status,
                x.ProgressCurrent,
                x.ProgressTarget,
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (row is null)
        {
            return null;
        }

        var campaign = await dbContext.Campaigns
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == row.CampaignId, cancellationToken);

        var tracking = row.TrackingTemplateId.HasValue
            ? await dbContext.TrackingTemplates.AsNoTracking().FirstOrDefaultAsync(x => x.Id == row.TrackingTemplateId.Value, cancellationToken)
            : null;

        var events = await dbContext.TrackingEvents
            .AsNoTracking()
            .Where(x => x.UserCampaignId == row.Id)
            .OrderByDescending(x => x.OccurredAtUtc)
            .Select(x => new MobileTrackingEventDto
            {
                Id = x.Id.ToString(),
                DateLabel = x.OccurredAtUtc.Date == DateTime.UtcNow.Date ? "Bugün" : x.OccurredAtUtc.ToString("dd MMM"),
                AmountText = x.AmountText,
                MerchantName = x.MerchantName,
                Qualified = x.Qualified,
            })
            .ToArrayAsync(cancellationToken);

        return new MobileTrackedCampaignDto
        {
            Id = row.TrackingTemplateId ?? row.Id,
            Title = tracking?.Title ?? campaign?.Title ?? "Takip Kampanyası",
            BankName = tracking?.BankName ?? campaign?.BankName ?? string.Empty,
            ProgressCurrent = row.ProgressCurrent,
            ProgressTarget = row.ProgressTarget,
            DeadlineText = campaign?.DeadlineText ?? "Yakında sona eriyor",
            RewardText = tracking?.RewardText ?? campaign?.RewardText ?? string.Empty,
            ShortDescription = tracking?.Description ?? campaign?.ShortDescription ?? string.Empty,
            NextActionText = tracking?.NextActionText ?? campaign?.NextActionText ?? string.Empty,
            Status = MapStatus(row.Status),
            Requirements = BuildRequirements(tracking?.RequirementText, row.ProgressCurrent, row.ProgressTarget),
            Events = events,
        };
    }

    private static TrackingProgressStatus MapStatus(UserCampaignStatus status)
        => status switch
        {
            UserCampaignStatus.Completed => TrackingProgressStatus.Completed,
            UserCampaignStatus.RewardPending => TrackingProgressStatus.Completed,
            UserCampaignStatus.Rewarded => TrackingProgressStatus.Completed,
            UserCampaignStatus.InProgress => TrackingProgressStatus.NearComplete,
            _ => TrackingProgressStatus.Active,
        };

    private static IReadOnlyCollection<string> BuildRequirements(string? requirementText, int progressCurrent, int progressTarget)
    {
        var items = new List<string>();

        if (!string.IsNullOrWhiteSpace(requirementText))
        {
            items.Add(requirementText);
        }

        items.Add($"İlerleme: {progressCurrent}/{progressTarget}");

        return items;
    }
}
