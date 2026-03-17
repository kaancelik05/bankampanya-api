namespace Bankampanya.Application.Features.MobileProfile.Dtos;

public sealed class MobileProfileDto
{
    public MobileProfileSummaryDto Summary { get; init; } = new();
    public IReadOnlyCollection<MobileProfileMenuGroupDto> MenuGroups { get; init; } = [];
}
