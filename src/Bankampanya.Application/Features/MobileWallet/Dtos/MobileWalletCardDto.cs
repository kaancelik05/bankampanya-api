namespace Bankampanya.Application.Features.MobileWallet.Dtos;

public sealed class MobileWalletCardDto
{
    public string Id { get; init; } = string.Empty;
    public string BankName { get; init; } = string.Empty;
    public string CardType { get; init; } = string.Empty;
    public string CustomName { get; init; } = string.Empty;
    public bool IsActive { get; init; }
}
