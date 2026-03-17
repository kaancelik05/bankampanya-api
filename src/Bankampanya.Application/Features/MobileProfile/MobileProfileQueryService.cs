using Bankampanya.Application.Features.MobileProfile.Dtos;

namespace Bankampanya.Application.Features.MobileProfile;

public sealed class MobileProfileQueryService(IMobileProfileQueryRepository repository)
{
    public Task<MobileProfileDto> GetAsync(CancellationToken cancellationToken)
        => repository.GetAsync(cancellationToken);
}
