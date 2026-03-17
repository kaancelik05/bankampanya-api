using Bankampanya.Domain.Enums;

namespace Bankampanya.Application.Features.Credits.Dtos;

public sealed class CreditAdminListItemDto
{
    public Guid Id { get; init; }
    public string BankName { get; init; } = string.Empty;
    public string Title { get; init; } = string.Empty;
    public CreditOfferType Type { get; init; }
    public CreditOfferSubtype? Subtype { get; init; }
    public string Rate { get; init; } = string.Empty;
    public string AmountRange { get; init; } = string.Empty;
    public string DetailSummary { get; init; } = string.Empty;
    public IReadOnlyCollection<string> Terms { get; init; } = [];
    public PublishStatus Status { get; init; }
    public DateTime CreatedAtUtc { get; init; }
    public DateTime UpdatedAtUtc { get; init; }
    public DateTime? PublishedAtUtc { get; init; }
}
