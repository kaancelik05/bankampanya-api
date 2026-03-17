using Bankampanya.Application.Common.Exceptions;
using Bankampanya.Application.Features.Tracking;
using Bankampanya.Application.Features.Tracking.Dtos;
using Bankampanya.Domain.Entities;
using Bankampanya.Domain.Enums;
using Bankampanya.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Bankampanya.Infrastructure.Persistence.Repositories;

public sealed class TrackingAdminRepository(AppDbContext dbContext) : ITrackingAdminRepository
{
    public async Task<IReadOnlyList<TrackingAdminListItemDto>> GetListAsync(TrackingListQuery query, CancellationToken cancellationToken)
    {
        var trackingQuery = dbContext.TrackingTemplates
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.Status) && Enum.TryParse<PublishStatus>(query.Status, true, out var status))
        {
            trackingQuery = trackingQuery.Where(x => x.Status == status);
        }

        if (!string.IsNullOrWhiteSpace(query.ProgressStatus) && Enum.TryParse<TrackingProgressStatus>(query.ProgressStatus, true, out var progressStatus))
        {
            trackingQuery = trackingQuery.Where(x => x.ProgressStatus == progressStatus);
        }

        if (query.CampaignId.HasValue)
        {
            trackingQuery = trackingQuery.Where(x => x.CampaignId == query.CampaignId.Value);
        }

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var search = query.Search.Trim();
            trackingQuery = trackingQuery.Where(x =>
                x.Title.Contains(search) ||
                x.BankName.Contains(search) ||
                x.Description.Contains(search) ||
                x.RequirementText.Contains(search));
        }

        var trackingItems = await trackingQuery
            .OrderByDescending(x => x.UpdatedAtUtc)
            .ToListAsync(cancellationToken);

        return trackingItems.Select(MapToDto).ToList();
    }

    public async Task<TrackingAdminListItemDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var tracking = await dbContext.TrackingTemplates
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        return tracking is null ? null : MapToDto(tracking);
    }

    public async Task<TrackingAdminListItemDto> CreateAsync(UpsertTrackingRequest request, CancellationToken cancellationToken)
    {
        var campaignExists = await dbContext.Campaigns
            .AnyAsync(x => x.Id == request.CampaignId, cancellationToken);

        if (!campaignExists)
        {
            throw new DomainValidationException("Tracking tanımı için verilen CampaignId bulunamadı.");
        }

        var now = DateTime.UtcNow;
        var tracking = new TrackingTemplate
        {
            Id = Guid.NewGuid(),
            CampaignId = request.CampaignId,
            BankName = request.BankName.Trim(),
            Title = request.Title.Trim(),
            Description = request.Description.Trim(),
            RequirementText = request.RequirementText.Trim(),
            NextActionText = request.NextActionText.Trim(),
            RewardText = request.RewardText.Trim(),
            DefaultProgressTarget = request.DefaultProgressTarget,
            ProgressStatus = request.ProgressStatus,
            Status = request.Status,
            CreatedAtUtc = now,
            UpdatedAtUtc = now,
            PublishedAtUtc = request.Status == PublishStatus.Live ? now : null,
        };

        dbContext.TrackingTemplates.Add(tracking);
        await dbContext.SaveChangesAsync(cancellationToken);

        return MapToDto(tracking);
    }

    public async Task<TrackingAdminListItemDto?> UpdateAsync(Guid id, UpsertTrackingRequest request, CancellationToken cancellationToken)
    {
        var tracking = await dbContext.TrackingTemplates
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (tracking is null)
        {
            return null;
        }

        var campaignExists = await dbContext.Campaigns
            .AnyAsync(x => x.Id == request.CampaignId, cancellationToken);

        if (!campaignExists)
        {
            throw new DomainValidationException("Tracking tanımı için verilen CampaignId bulunamadı.");
        }

        var now = DateTime.UtcNow;
        tracking.CampaignId = request.CampaignId;
        tracking.BankName = request.BankName.Trim();
        tracking.Title = request.Title.Trim();
        tracking.Description = request.Description.Trim();
        tracking.RequirementText = request.RequirementText.Trim();
        tracking.NextActionText = request.NextActionText.Trim();
        tracking.RewardText = request.RewardText.Trim();
        tracking.DefaultProgressTarget = request.DefaultProgressTarget;
        tracking.ProgressStatus = request.ProgressStatus;
        tracking.Status = request.Status;
        tracking.UpdatedAtUtc = now;
        tracking.PublishedAtUtc ??= request.Status == PublishStatus.Live ? now : null;

        await dbContext.SaveChangesAsync(cancellationToken);

        return MapToDto(tracking);
    }

    private static TrackingAdminListItemDto MapToDto(TrackingTemplate tracking)
    {
        return new TrackingAdminListItemDto
        {
            Id = tracking.Id,
            CampaignId = tracking.CampaignId,
            BankName = tracking.BankName,
            Title = tracking.Title,
            Description = tracking.Description,
            RequirementText = tracking.RequirementText,
            NextActionText = tracking.NextActionText,
            RewardText = tracking.RewardText,
            DefaultProgressTarget = tracking.DefaultProgressTarget,
            ProgressStatus = tracking.ProgressStatus,
            Status = tracking.Status,
            CreatedAtUtc = tracking.CreatedAtUtc,
            UpdatedAtUtc = tracking.UpdatedAtUtc,
            PublishedAtUtc = tracking.PublishedAtUtc,
        };
    }
}
