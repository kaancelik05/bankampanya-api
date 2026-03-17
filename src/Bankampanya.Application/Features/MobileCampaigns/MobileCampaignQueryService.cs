using Bankampanya.Application.Features.MobileCampaigns.Dtos;

namespace Bankampanya.Application.Features.MobileCampaigns;

public sealed class MobileCampaignQueryService(IMobileCampaignQueryRepository repository)
{
    public Task<IReadOnlyList<MobileCampaignListItemDto>> GetListAsync(MobileCampaignListQuery query, CancellationToken cancellationToken)
        => repository.GetListAsync(query, cancellationToken);

    public Task<MobileCampaignDetailDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        => repository.GetByIdAsync(id, cancellationToken);
}
