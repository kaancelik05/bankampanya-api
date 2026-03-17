namespace Bankampanya.Application.Features.Campaigns.Dtos;

public sealed class CampaignListQuery
{
    public string? Search { get; init; }
    public string? BankName { get; init; }
    public string? Category { get; init; }
    public string? Status { get; init; }
}
