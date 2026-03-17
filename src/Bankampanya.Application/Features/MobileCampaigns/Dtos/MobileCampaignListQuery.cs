namespace Bankampanya.Application.Features.MobileCampaigns.Dtos;

public sealed class MobileCampaignListQuery
{
    public string? Search { get; init; }
    public string? Category { get; init; }
    public string? BankName { get; init; }
}
