using Bankampanya.Application.Features.MobileWallet.Dtos;

namespace Bankampanya.Application.Features.MobileWallet;

public sealed class MobileWalletMutationService(IMobileWalletMutationRepository repository)
{
    public Task<MobileWalletCardDto> CreateAsync(CreateWalletCardRequest request, CancellationToken cancellationToken)
        => repository.CreateAsync(request, cancellationToken);

    public Task<MobileWalletCardDto?> UpdateStatusAsync(Guid id, UpdateWalletCardStatusRequest request, CancellationToken cancellationToken)
        => repository.UpdateStatusAsync(id, request, cancellationToken);

    public Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken)
        => repository.DeleteAsync(id, cancellationToken);
}
