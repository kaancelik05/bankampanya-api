using Bankampanya.Application.Features.Campaigns;
using Bankampanya.Application.Features.Campaigns.Dtos;
using Bankampanya.Domain.Entities;
using Bankampanya.Domain.Enums;
using Bankampanya.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Bankampanya.Infrastructure.Persistence.Repositories;

public sealed class CampaignAdminRepository(AppDbContext dbContext) : ICampaignAdminRepository
{
    public async Task<IReadOnlyList<CampaignAdminListItemDto>> GetListAsync(CampaignListQuery query, CancellationToken cancellationToken)
    {
        var campaignsQuery = dbContext.Campaigns
            .AsNoTracking()
            .Include(x => x.Terms.OrderBy(term => term.SortOrder))
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.Status) && Enum.TryParse<PublishStatus>(query.Status, true, out var status))
        {
            campaignsQuery = campaignsQuery.Where(x => x.Status == status);
        }

        if (!string.IsNullOrWhiteSpace(query.BankName))
        {
            var bankName = query.BankName.Trim();
            campaignsQuery = campaignsQuery.Where(x => x.BankName.Contains(bankName));
        }

        if (!string.IsNullOrWhiteSpace(query.Category))
        {
            var category = query.Category.Trim();
            campaignsQuery = campaignsQuery.Where(x => x.Category.Contains(category));
        }

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var search = query.Search.Trim();
            campaignsQuery = campaignsQuery.Where(x =>
                x.Title.Contains(search) ||
                x.BankName.Contains(search) ||
                x.ShortDescription.Contains(search));
        }

        var campaigns = await campaignsQuery
            .OrderByDescending(x => x.UpdatedAtUtc)
            .ToListAsync(cancellationToken);

        return campaigns.Select(MapToDto).ToList();
    }

    public async Task<CampaignAdminListItemDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var campaign = await dbContext.Campaigns
            .AsNoTracking()
            .Include(x => x.Terms.OrderBy(term => term.SortOrder))
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        return campaign is null ? null : MapToDto(campaign);
    }

    public async Task<CampaignAdminListItemDto> CreateAsync(UpsertCampaignRequest request, CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;
        var campaign = new Campaign
        {
            Id = Guid.NewGuid(),
            BankName = request.BankName.Trim(),
            Category = request.Category.Trim(),
            Title = request.Title.Trim(),
            ShortDescription = request.ShortDescription.Trim(),
            RewardText = request.RewardText.Trim(),
            RewardType = request.RewardType,
            DeadlineText = request.DeadlineText.Trim(),
            ValidFromUtc = request.ValidFromUtc,
            ValidToUtc = request.ValidToUtc,
            ValidDateRangeLabel = request.ValidDateRangeLabel.Trim(),
            Status = request.Status,
            IsProgressive = request.IsProgressive,
            ProgressTarget = request.ProgressTarget,
            NextActionText = string.IsNullOrWhiteSpace(request.NextActionText) ? null : request.NextActionText.Trim(),
            CreatedAtUtc = now,
            UpdatedAtUtc = now,
            PublishedAtUtc = request.Status == PublishStatus.Live ? now : null,
        };

        campaign.Terms = request.Terms
            .Where(term => !string.IsNullOrWhiteSpace(term))
            .Select((term, index) => new CampaignTerm
            {
                Id = Guid.NewGuid(),
                CampaignId = campaign.Id,
                SortOrder = index + 1,
                Text = term.Trim(),
                CreatedAtUtc = now,
                UpdatedAtUtc = now,
            })
            .ToList();

        dbContext.Campaigns.Add(campaign);
        await dbContext.SaveChangesAsync(cancellationToken);

        return MapToDto(campaign);
    }

    public async Task<CampaignAdminListItemDto?> UpdateAsync(Guid id, UpsertCampaignRequest request, CancellationToken cancellationToken)
    {
        var campaign = await dbContext.Campaigns
            .Include(x => x.Terms)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (campaign is null)
        {
            return null;
        }

        var now = DateTime.UtcNow;
        campaign.BankName = request.BankName.Trim();
        campaign.Category = request.Category.Trim();
        campaign.Title = request.Title.Trim();
        campaign.ShortDescription = request.ShortDescription.Trim();
        campaign.RewardText = request.RewardText.Trim();
        campaign.RewardType = request.RewardType;
        campaign.DeadlineText = request.DeadlineText.Trim();
        campaign.ValidFromUtc = request.ValidFromUtc;
        campaign.ValidToUtc = request.ValidToUtc;
        campaign.ValidDateRangeLabel = request.ValidDateRangeLabel.Trim();
        campaign.Status = request.Status;
        campaign.IsProgressive = request.IsProgressive;
        campaign.ProgressTarget = request.ProgressTarget;
        campaign.NextActionText = string.IsNullOrWhiteSpace(request.NextActionText) ? null : request.NextActionText.Trim();
        campaign.UpdatedAtUtc = now;
        campaign.PublishedAtUtc ??= request.Status == PublishStatus.Live ? now : null;

        dbContext.CampaignTerms.RemoveRange(campaign.Terms);

        campaign.Terms = request.Terms
            .Where(term => !string.IsNullOrWhiteSpace(term))
            .Select((term, index) => new CampaignTerm
            {
                Id = Guid.NewGuid(),
                CampaignId = campaign.Id,
                SortOrder = index + 1,
                Text = term.Trim(),
                CreatedAtUtc = now,
                UpdatedAtUtc = now,
            })
            .ToList();

        await dbContext.SaveChangesAsync(cancellationToken);

        return MapToDto(campaign);
    }

    private static CampaignAdminListItemDto MapToDto(Campaign campaign)
    {
        return new CampaignAdminListItemDto
        {
            Id = campaign.Id,
            BankName = campaign.BankName,
            Category = campaign.Category,
            Title = campaign.Title,
            ShortDescription = campaign.ShortDescription,
            RewardText = campaign.RewardText,
            RewardType = campaign.RewardType,
            DeadlineText = campaign.DeadlineText,
            ValidDateRangeLabel = campaign.ValidDateRangeLabel,
            Status = campaign.Status,
            IsProgressive = campaign.IsProgressive,
            ProgressTarget = campaign.ProgressTarget,
            NextActionText = campaign.NextActionText,
            Terms = campaign.Terms.OrderBy(x => x.SortOrder).Select(x => x.Text).ToArray(),
            CreatedAtUtc = campaign.CreatedAtUtc,
            UpdatedAtUtc = campaign.UpdatedAtUtc,
            PublishedAtUtc = campaign.PublishedAtUtc,
        };
    }
}
