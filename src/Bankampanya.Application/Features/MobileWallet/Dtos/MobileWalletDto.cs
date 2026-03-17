namespace Bankampanya.Application.Features.MobileWallet.Dtos;

public sealed class MobileWalletDto
{
    public IReadOnlyCollection<MobileWalletCardDto> Cards { get; init; } = [];
}
