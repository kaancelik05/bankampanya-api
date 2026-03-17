namespace Bankampanya.Application.Features.MobileProfile.Dtos;

public sealed class MobileProfileMenuGroupDto
{
    public string Id { get; init; } = string.Empty;
    public string Title { get; init; } = string.Empty;
    public IReadOnlyCollection<MobileProfileMenuItemDto> Items { get; init; } = [];
}
