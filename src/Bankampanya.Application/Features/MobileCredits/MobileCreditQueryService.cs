using Bankampanya.Application.Features.MobileCredits.Dtos;

namespace Bankampanya.Application.Features.MobileCredits;

public sealed class MobileCreditQueryService(IMobileCreditQueryRepository repository)
{
    public Task<IReadOnlyList<MobileCreditListItemDto>> GetListAsync(MobileCreditListQuery query, CancellationToken cancellationToken)
        => repository.GetListAsync(query, cancellationToken);

    public Task<MobileCreditDetailDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        => repository.GetByIdAsync(id, cancellationToken);
}
