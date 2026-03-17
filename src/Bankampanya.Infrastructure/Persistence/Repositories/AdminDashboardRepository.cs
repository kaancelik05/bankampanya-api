using Bankampanya.Application.Features.Dashboard;
using Bankampanya.Application.Features.Dashboard.Dtos;
using Bankampanya.Domain.Enums;
using Bankampanya.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Bankampanya.Infrastructure.Persistence.Repositories;

public sealed class AdminDashboardRepository(AppDbContext dbContext) : IAdminDashboardRepository
{
    public async Task<AdminDashboardSummaryDto> GetSummaryAsync(CancellationToken cancellationToken)
    {
        return new AdminDashboardSummaryDto
        {
            LiveCampaignCount = await dbContext.Campaigns.CountAsync(x => x.Status == PublishStatus.Live, cancellationToken),
            DraftCampaignCount = await dbContext.Campaigns.CountAsync(x => x.Status == PublishStatus.Draft, cancellationToken),
            ProgressiveCampaignCount = await dbContext.Campaigns.CountAsync(x => x.IsProgressive, cancellationToken),
            LiveCreditCount = await dbContext.CreditOffers.CountAsync(x => x.Status == PublishStatus.Live, cancellationToken),
            DraftCreditCount = await dbContext.CreditOffers.CountAsync(x => x.Status == PublishStatus.Draft, cancellationToken),
            LiveNotificationCount = await dbContext.NotificationTemplates.CountAsync(x => x.Status == PublishStatus.Live, cancellationToken),
            DraftNotificationCount = await dbContext.NotificationTemplates.CountAsync(x => x.Status == PublishStatus.Draft, cancellationToken),
            ActiveTrackingCount = await dbContext.TrackingTemplates.CountAsync(x => x.ProgressStatus == TrackingProgressStatus.Active, cancellationToken),
            NearCompleteTrackingCount = await dbContext.TrackingTemplates.CountAsync(x => x.ProgressStatus == TrackingProgressStatus.NearComplete, cancellationToken),
            LiveAssistantPromptCount = await dbContext.AssistantPromptTemplates.CountAsync(x => x.Status == PublishStatus.Live, cancellationToken),
            DraftAssistantPromptCount = await dbContext.AssistantPromptTemplates.CountAsync(x => x.Status == PublishStatus.Draft, cancellationToken),
        };
    }
}
