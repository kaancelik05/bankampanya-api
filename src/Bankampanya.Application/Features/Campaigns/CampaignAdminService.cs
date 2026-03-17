using Bankampanya.Application.Features.Campaigns.Dtos;

namespace Bankampanya.Application.Features.Campaigns;

public sealed class CampaignAdminService(ICampaignAdminRepository repository)
{
    public Task<IReadOnlyList<CampaignAdminListItemDto>> GetListAsync(CampaignListQuery query, CancellationToken cancellationToken)
        => repository.GetListAsync(query, cancellationToken);

    public Task<CampaignAdminListItemDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        => repository.GetByIdAsync(id, cancellationToken);

    public Task<CampaignAdminListItemDto> CreateAsync(UpsertCampaignRequest request, CancellationToken cancellationToken)
        => repository.CreateAsync(request, cancellationToken);

    public Task<CampaignAdminListItemDto?> UpdateAsync(Guid id, UpsertCampaignRequest request, CancellationToken cancellationToken)
        => repository.UpdateAsync(id, request, cancellationToken);
}
