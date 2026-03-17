using Bankampanya.Application.Features.MobileWallet.Dtos;

namespace Bankampanya.Application.Features.MobileWallet;

public interface IMobileWalletQueryRepository
{
    Task<MobileWalletDto> GetAsync(CancellationToken cancellationToken);
}
