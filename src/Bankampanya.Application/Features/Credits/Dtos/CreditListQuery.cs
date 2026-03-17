namespace Bankampanya.Application.Features.Credits.Dtos;

public sealed class CreditListQuery
{
    public string? Search { get; init; }
    public string? Type { get; init; }
    public string? Status { get; init; }
}
