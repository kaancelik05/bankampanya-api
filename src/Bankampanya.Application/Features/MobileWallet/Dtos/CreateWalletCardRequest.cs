namespace Bankampanya.Application.Features.MobileWallet.Dtos;

public sealed class CreateWalletCardRequest
{
    public string BankName { get; init; } = string.Empty;
    public string CardType { get; init; } = string.Empty;
    public string CustomName { get; init; } = string.Empty;
}
