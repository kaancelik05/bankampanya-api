using Bankampanya.Application.Features.MobileCampaignJoin.Dtos;

namespace Bankampanya.Application.Features.MobileCampaignJoin;

public interface IMobileCampaignJoinRepository
{
    Task<JoinedCampaignDto> JoinAsync(Guid campaignId, CancellationToken cancellationToken);
}
