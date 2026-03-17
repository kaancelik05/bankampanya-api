using Bankampanya.Domain.Common;
using Bankampanya.Domain.Enums;

namespace Bankampanya.Domain.Entities;

public class CreditOffer : PublishableEntity
{
    public string BankName { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public CreditOfferType Type { get; set; }
    public CreditOfferSubtype? Subtype { get; set; }
    public string Rate { get; set; } = string.Empty;
    public string AmountRange { get; set; } = string.Empty;
    public string DetailSummary { get; set; } = string.Empty;
    public List<string> Terms { get; set; } = new();
}
