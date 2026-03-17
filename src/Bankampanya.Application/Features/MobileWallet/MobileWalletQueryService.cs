using Bankampanya.Application.Features.MobileWallet.Dtos;

namespace Bankampanya.Application.Features.MobileWallet;

public sealed class MobileWalletQueryService(IMobileWalletQueryRepository repository)
{
    public Task<MobileWalletDto> GetAsync(CancellationToken cancellationToken)
        => repository.GetAsync(cancellationToken);
}
