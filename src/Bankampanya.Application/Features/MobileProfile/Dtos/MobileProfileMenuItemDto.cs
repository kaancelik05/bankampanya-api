namespace Bankampanya.Application.Features.MobileProfile.Dtos;

public sealed class MobileProfileMenuItemDto
{
    public string Id { get; init; } = string.Empty;
    public string Title { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string? Route { get; init; }
}
