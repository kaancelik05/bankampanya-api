using Bankampanya.Domain.Enums;

namespace Bankampanya.Application.Features.MobileCampaigns.Dtos;

public sealed class MobileCampaignDetailDto
{
    public Guid Id { get; init; }
    public string BankName { get; init; } = string.Empty;
    public string Title { get; init; } = string.Empty;
    public string ShortDescription { get; init; } = string.Empty;
    public string RewardText { get; init; } = string.Empty;
    public RewardType RewardType { get; init; }
    public string Category { get; init; } = string.Empty;
    public string DeadlineText { get; init; } = string.Empty;
    public string ValidDateRange { get; init; } = string.Empty;
    public string? NextActionText { get; init; }
    public IReadOnlyCollection<string> Terms { get; init; } = [];
    public bool IsJoined { get; init; }
    public bool IsProgressive { get; init; }
    public int? ProgressCurrent { get; init; }
    public int? ProgressTarget { get; init; }
    public IReadOnlyCollection<MobileCampaignTagDto> Tags { get; init; } = [];
}
