using Bankampanya.Domain.Enums;

namespace Bankampanya.Application.Features.MobileCredits.Dtos;

public sealed class MobileCreditDetailDto
{
    public Guid Id { get; init; }
    public string Title { get; init; } = string.Empty;
    public string BankName { get; init; } = string.Empty;
    public string Rate { get; init; } = string.Empty;
    public string AmountRange { get; init; } = string.Empty;
    public CreditOfferType Type { get; init; }
    public CreditOfferSubtype? Subtype { get; init; }
    public string DetailSummary { get; init; } = string.Empty;
    public IReadOnlyCollection<string> Terms { get; init; } = [];
}
