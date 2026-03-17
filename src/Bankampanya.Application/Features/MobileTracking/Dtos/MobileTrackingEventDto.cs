using Bankampanya.Domain.Enums;

namespace Bankampanya.Application.Features.MobileTracking.Dtos;

public sealed class MobileTrackingEventDto
{
    public string Id { get; init; } = string.Empty;
    public string DateLabel { get; init; } = string.Empty;
    public string AmountText { get; init; } = string.Empty;
    public string MerchantName { get; init; } = string.Empty;
    public bool Qualified { get; init; }
}
