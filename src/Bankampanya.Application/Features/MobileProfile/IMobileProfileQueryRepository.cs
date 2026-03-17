using Bankampanya.Application.Features.MobileProfile.Dtos;

namespace Bankampanya.Application.Features.MobileProfile;

public interface IMobileProfileQueryRepository
{
    Task<MobileProfileDto> GetAsync(CancellationToken cancellationToken);
}
