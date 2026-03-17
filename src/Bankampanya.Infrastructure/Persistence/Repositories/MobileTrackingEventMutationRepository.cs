using Bankampanya.Application.Common.Exceptions;
using Bankampanya.Application.Common.Interfaces;
using Bankampanya.Application.Features.MobileTrackingEvents;
using Bankampanya.Application.Features.MobileTrackingEvents.Dtos;
using Bankampanya.Domain.Entities;
using Bankampanya.Domain.Enums;
using Bankampanya.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Bankampanya.Infrastructure.Persistence.Repositories;

public sealed class MobileTrackingEventMutationRepository(
    AppDbContext dbContext,
    ICurrentUserService currentUserService,
    ILogger<MobileTrackingEventMutationRepository> logger) : IMobileTrackingEventMutationRepository
{
    public async Task<TrackingEventMutationResultDto> CreateAsync(
        Guid trackingTemplateId,
        CreateTrackingEventRequest request,
        CancellationToken cancellationToken)
    {
        var userId = currentUserService.GetUserId();

        var userCampaign = await dbContext.UserCampaigns
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(
                x => x.UserId == userId && x.TrackingTemplateId == trackingTemplateId,
                cancellationToken);

        if (userCampaign is null)
        {
            userCampaign = await dbContext.UserCampaigns
                .OrderByDescending(x => x.UpdatedAtUtc)
                .FirstOrDefaultAsync(
                    x => x.UserId == userId && x.CampaignId == trackingTemplateId,
                    cancellationToken);
        }

        if (userCampaign is null)
        {
            throw new EntityNotFoundException($"Tracking campaign '{trackingTemplateId}' not found for current user.");
        }

        var eventEntity = new TrackingEvent
        {
            Id = Guid.NewGuid(),
            UserCampaignId = userCampaign.Id,
            MerchantName = request.MerchantName,
            Amount = request.Amount,
            AmountText = request.AmountText,
            Qualified = true,
            Note = request.Note,
            OccurredAtUtc = DateTime.UtcNow,
            CreatedAtUtc = DateTime.UtcNow,
            UpdatedAtUtc = DateTime.UtcNow,
        };

        dbContext.TrackingEvents.Add(eventEntity);

        if (userCampaign.ProgressCurrent < userCampaign.ProgressTarget)
        {
            userCampaign.ProgressCurrent += 1;
        }

        userCampaign.Status = userCampaign.ProgressCurrent >= userCampaign.ProgressTarget
            ? UserCampaignStatus.Completed
            : UserCampaignStatus.InProgress;

        if (userCampaign.Status == UserCampaignStatus.Completed && userCampaign.CompletedAtUtc is null)
        {
            userCampaign.CompletedAtUtc = DateTime.UtcNow;
        }

        userCampaign.UpdatedAtUtc = DateTime.UtcNow;

        dbContext.UserCampaigns.Attach(userCampaign);
        var userCampaignEntry = dbContext.Entry(userCampaign);
        userCampaignEntry.Property(x => x.ProgressCurrent).IsModified = true;
        userCampaignEntry.Property(x => x.Status).IsModified = true;
        userCampaignEntry.Property(x => x.CompletedAtUtc).IsModified = true;
        userCampaignEntry.Property(x => x.UpdatedAtUtc).IsModified = true;

        logger.LogInformation(
            "Updating user campaign {UserCampaignId} progress to {ProgressCurrent}/{ProgressTarget} with status {Status}",
            userCampaign.Id,
            userCampaign.ProgressCurrent,
            userCampaign.ProgressTarget,
            userCampaign.Status);

        await dbContext.SaveChangesAsync(cancellationToken);
        await dbContext.Entry(userCampaign).ReloadAsync(cancellationToken);

        return new TrackingEventMutationResultDto
        {
            EventId = eventEntity.Id,
            UserCampaignId = userCampaign.Id,
            ProgressCurrent = userCampaign.ProgressCurrent,
            ProgressTarget = userCampaign.ProgressTarget,
            Qualified = eventEntity.Qualified,
            Status = userCampaign.Status.ToString(),
        };
    }
}
