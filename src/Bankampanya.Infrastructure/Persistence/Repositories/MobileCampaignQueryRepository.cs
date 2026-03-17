using Bankampanya.Application.Features.MobileCampaigns;
using Bankampanya.Application.Features.MobileCampaigns.Dtos;
using Bankampanya.Domain.Enums;
using Bankampanya.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Bankampanya.Infrastructure.Persistence.Repositories;

public sealed class MobileCampaignQueryRepository(AppDbContext dbContext) : IMobileCampaignQueryRepository
{
    public async Task<IReadOnlyList<MobileCampaignListItemDto>> GetListAsync(MobileCampaignListQuery query, CancellationToken cancellationToken)
    {
        var campaignsQuery = dbContext.Campaigns
            .AsNoTracking()
            .Include(x => x.Terms.OrderBy(term => term.SortOrder))
            .Where(x => x.Status == PublishStatus.Live)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.Category))
        {
            var category = query.Category.Trim();
            campaignsQuery = campaignsQuery.Where(x => x.Category.Contains(category));
        }

        if (!string.IsNullOrWhiteSpace(query.BankName))
        {
            var bankName = query.BankName.Trim();
            campaignsQuery = campaignsQuery.Where(x => x.BankName.Contains(bankName));
        }

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var search = query.Search.Trim();
            campaignsQuery = campaignsQuery.Where(x =>
                x.Title.Contains(search) ||
                x.BankName.Contains(search) ||
                x.ShortDescription.Contains(search) ||
                x.Category.Contains(search));
        }

        var campaigns = await campaignsQuery
            .OrderByDescending(x => x.UpdatedAtUtc)
            .ToListAsync(cancellationToken);

        return campaigns.Select(MapListItem).ToList();
    }

    public async Task<MobileCampaignDetailDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var campaign = await dbContext.Campaigns
            .AsNoTracking()
            .Include(x => x.Terms.OrderBy(term => term.SortOrder))
            .FirstOrDefaultAsync(x => x.Id == id && x.Status == PublishStatus.Live, cancellationToken);

        return campaign is null ? null : MapDetail(campaign);
    }

    private static MobileCampaignListItemDto MapListItem(Domain.Entities.Campaign campaign)
    {
        return new MobileCampaignListItemDto
        {
            Id = campaign.Id,
            BankName = campaign.BankName,
            Title = campaign.Title,
            ShortDescription = campaign.ShortDescription,
            RewardText = campaign.RewardText,
            RewardType = campaign.RewardType,
            Category = campaign.Category,
            DeadlineText = campaign.DeadlineText,
            IsJoined = false,
            IsProgressive = campaign.IsProgressive,
            ProgressCurrent = campaign.IsProgressive ? 0 : null,
            ProgressTarget = campaign.ProgressTarget,
            Tags = BuildTags(campaign),
        };
    }

    private static MobileCampaignDetailDto MapDetail(Domain.Entities.Campaign campaign)
    {
        return new MobileCampaignDetailDto
        {
            Id = campaign.Id,
            BankName = campaign.BankName,
            Title = campaign.Title,
            ShortDescription = campaign.ShortDescription,
            RewardText = campaign.RewardText,
            RewardType = campaign.RewardType,
            Category = campaign.Category,
            DeadlineText = campaign.DeadlineText,
            ValidDateRange = campaign.ValidDateRangeLabel,
            NextActionText = campaign.NextActionText,
            Terms = campaign.Terms.OrderBy(x => x.SortOrder).Select(x => x.Text).ToArray(),
            IsJoined = false,
            IsProgressive = campaign.IsProgressive,
            ProgressCurrent = campaign.IsProgressive ? 0 : null,
            ProgressTarget = campaign.ProgressTarget,
            Tags = BuildTags(campaign),
        };
    }

    private static IReadOnlyCollection<MobileCampaignTagDto> BuildTags(Domain.Entities.Campaign campaign)
    {
        var tags = new List<MobileCampaignTagDto>();

        if (campaign.IsProgressive)
        {
            tags.Add(new MobileCampaignTagDto
            {
                Id = "progressive",
                Label = "Adımlı Kampanya",
                Tone = "info",
            });
        }

        tags.Add(new MobileCampaignTagDto
        {
            Id = campaign.Status.ToString().ToLowerInvariant(),
            Label = campaign.Status == PublishStatus.Live ? "Yayında" : campaign.Status.ToString(),
            Tone = campaign.Status == PublishStatus.Live ? "success" : "neutral",
        });

        return tags;
    }
}
