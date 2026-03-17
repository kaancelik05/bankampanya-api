using Bankampanya.Application.Features.MobileWallet.Dtos;

namespace Bankampanya.Application.Features.MobileWallet;

public interface IMobileWalletMutationRepository
{
    Task<MobileWalletCardDto> CreateAsync(CreateWalletCardRequest request, CancellationToken cancellationToken);
    Task<MobileWalletCardDto?> UpdateStatusAsync(Guid id, UpdateWalletCardStatusRequest request, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken);
}
