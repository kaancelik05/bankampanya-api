namespace Bankampanya.Application.Features.Dashboard.Dtos;

public sealed class AdminDashboardSummaryDto
{
    public int LiveCampaignCount { get; init; }
    public int DraftCampaignCount { get; init; }
    public int ProgressiveCampaignCount { get; init; }
    public int LiveCreditCount { get; init; }
    public int DraftCreditCount { get; init; }
    public int LiveNotificationCount { get; init; }
    public int DraftNotificationCount { get; init; }
    public int ActiveTrackingCount { get; init; }
    public int NearCompleteTrackingCount { get; init; }
    public int LiveAssistantPromptCount { get; init; }
    public int DraftAssistantPromptCount { get; init; }
}
