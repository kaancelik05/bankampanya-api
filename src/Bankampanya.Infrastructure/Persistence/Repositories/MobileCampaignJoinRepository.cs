using Bankampanya.Application.Common.Exceptions;
using Bankampanya.Application.Common.Interfaces;
using Bankampanya.Application.Features.MobileCampaignJoin;
using Bankampanya.Application.Features.MobileCampaignJoin.Dtos;
using Bankampanya.Domain.Entities;
using Bankampanya.Domain.Enums;
using Bankampanya.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Bankampanya.Infrastructure.Persistence.Repositories;

public sealed class MobileCampaignJoinRepository(
    AppDbContext dbContext,
    ICurrentUserService currentUserService) : IMobileCampaignJoinRepository
{
    public async Task<JoinedCampaignDto> JoinAsync(Guid campaignId, CancellationToken cancellationToken)
    {
        var userId = currentUserService.GetUserId();

        var campaign = await dbContext.Campaigns
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == campaignId && x.Status == PublishStatus.Live, cancellationToken);

        if (campaign is null)
        {
            throw new EntityNotFoundException($"Campaign '{campaignId}' not found.");
        }

        var existing = await dbContext.UserCampaigns
            .AsNoTracking()
            .Where(x => x.UserId == userId && x.CampaignId == campaignId)
            .OrderByDescending(x => x.UpdatedAtUtc)
            .FirstOrDefaultAsync(cancellationToken);

        if (existing is not null)
        {
            return Map(existing);
        }

        var trackingTemplate = await dbContext.TrackingTemplates
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.CampaignId == campaignId && x.Status == PublishStatus.Live, cancellationToken);

        var progressTarget = trackingTemplate?.DefaultProgressTarget ?? campaign.ProgressTarget ?? 1;

        var entity = new UserCampaign
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            CampaignId = campaignId,
            TrackingTemplateId = trackingTemplate?.Id,
            Status = progressTarget > 1 ? UserCampaignStatus.InProgress : UserCampaignStatus.Joined,
            ProgressCurrent = 0,
            ProgressTarget = progressTarget,
            JoinedAtUtc = DateTime.UtcNow,
            CreatedAtUtc = DateTime.UtcNow,
            UpdatedAtUtc = DateTime.UtcNow,
        };

        dbContext.UserCampaigns.Add(entity);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Map(entity);
    }

    private static JoinedCampaignDto Map(UserCampaign entity)
        => new()
        {
            Id = entity.Id,
            CampaignId = entity.CampaignId,
            TrackingTemplateId = entity.TrackingTemplateId,
            Status = entity.Status.ToString(),
            ProgressCurrent = entity.ProgressCurrent,
            ProgressTarget = entity.ProgressTarget,
            JoinedAtLabel = "Bugün katıldı",
        };
}
