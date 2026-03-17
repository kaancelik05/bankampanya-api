using Bankampanya.Application.Features.MobileCampaignJoin.Dtos;

namespace Bankampanya.Application.Features.MobileCampaignJoin;

public sealed class MobileCampaignJoinService(IMobileCampaignJoinRepository repository)
{
    public Task<JoinedCampaignDto> JoinAsync(Guid campaignId, CancellationToken cancellationToken)
        => repository.JoinAsync(campaignId, cancellationToken);
}
