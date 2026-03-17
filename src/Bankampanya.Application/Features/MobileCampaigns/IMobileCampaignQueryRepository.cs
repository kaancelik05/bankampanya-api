using Bankampanya.Application.Features.MobileCampaigns.Dtos;

namespace Bankampanya.Application.Features.MobileCampaigns;

public interface IMobileCampaignQueryRepository
{
    Task<IReadOnlyList<MobileCampaignListItemDto>> GetListAsync(MobileCampaignListQuery query, CancellationToken cancellationToken);
    Task<MobileCampaignDetailDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
}
