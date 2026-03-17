using Bankampanya.Domain.Common;
using Bankampanya.Domain.Enums;

namespace Bankampanya.Domain.Entities;

public class WalletCard : BaseEntity
{
    public Guid UserId { get; set; }
    public string BankName { get; set; } = string.Empty;
    public string CardType { get; set; } = string.Empty;
    public string CustomName { get; set; } = string.Empty;
    public WalletCardStatus Status { get; set; }
}
