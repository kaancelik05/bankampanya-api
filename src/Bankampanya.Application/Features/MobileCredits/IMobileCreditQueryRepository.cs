using Bankampanya.Application.Features.MobileCredits.Dtos;

namespace Bankampanya.Application.Features.MobileCredits;

public interface IMobileCreditQueryRepository
{
    Task<IReadOnlyList<MobileCreditListItemDto>> GetListAsync(MobileCreditListQuery query, CancellationToken cancellationToken);
    Task<MobileCreditDetailDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
}
