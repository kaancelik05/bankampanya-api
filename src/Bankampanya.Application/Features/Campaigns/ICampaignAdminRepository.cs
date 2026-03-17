using Bankampanya.Application.Features.Campaigns.Dtos;

namespace Bankampanya.Application.Features.Campaigns;

public interface ICampaignAdminRepository
{
    Task<IReadOnlyList<CampaignAdminListItemDto>> GetListAsync(CampaignListQuery query, CancellationToken cancellationToken);
    Task<CampaignAdminListItemDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<CampaignAdminListItemDto> CreateAsync(UpsertCampaignRequest request, CancellationToken cancellationToken);
    Task<CampaignAdminListItemDto?> UpdateAsync(Guid id, UpsertCampaignRequest request, CancellationToken cancellationToken);
}
