using Bankampanya.Application.Features.Credits.Dtos;

namespace Bankampanya.Application.Features.Credits;

public sealed class CreditAdminService(ICreditAdminRepository repository)
{
    public Task<IReadOnlyList<CreditAdminListItemDto>> GetListAsync(CreditListQuery query, CancellationToken cancellationToken)
        => repository.GetListAsync(query, cancellationToken);

    public Task<CreditAdminListItemDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        => repository.GetByIdAsync(id, cancellationToken);

    public Task<CreditAdminListItemDto> CreateAsync(UpsertCreditRequest request, CancellationToken cancellationToken)
        => repository.CreateAsync(request, cancellationToken);

    public Task<CreditAdminListItemDto?> UpdateAsync(Guid id, UpsertCreditRequest request, CancellationToken cancellationToken)
        => repository.UpdateAsync(id, request, cancellationToken);
}
