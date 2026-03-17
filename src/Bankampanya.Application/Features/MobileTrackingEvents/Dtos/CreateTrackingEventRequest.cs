namespace Bankampanya.Application.Features.MobileTrackingEvents.Dtos;

public sealed class CreateTrackingEventRequest
{
    public string MerchantName { get; init; } = string.Empty;
    public decimal? Amount { get; init; }
    public string AmountText { get; init; } = string.Empty;
    public string? Note { get; init; }
}
