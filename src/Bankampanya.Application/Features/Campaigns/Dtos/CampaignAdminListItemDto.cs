using Bankampanya.Domain.Enums;

namespace Bankampanya.Application.Features.Campaigns.Dtos;

public sealed class CampaignAdminListItemDto
{
    public Guid Id { get; init; }
    public string BankName { get; init; } = string.Empty;
    public string Category { get; init; } = string.Empty;
    public string Title { get; init; } = string.Empty;
    public string ShortDescription { get; init; } = string.Empty;
    public string RewardText { get; init; } = string.Empty;
    public RewardType RewardType { get; init; }
    public string DeadlineText { get; init; } = string.Empty;
    public string ValidDateRangeLabel { get; init; } = string.Empty;
    public PublishStatus Status { get; init; }
    public bool IsProgressive { get; init; }
    public int? ProgressTarget { get; init; }
    public string? NextActionText { get; init; }
    public IReadOnlyCollection<string> Terms { get; init; } = [];
    public DateTime CreatedAtUtc { get; init; }
    public DateTime UpdatedAtUtc { get; init; }
    public DateTime? PublishedAtUtc { get; init; }
}
