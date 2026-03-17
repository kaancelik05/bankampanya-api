using Bankampanya.Application.Features.Credits.Dtos;

namespace Bankampanya.Application.Features.Credits;

public interface ICreditAdminRepository
{
    Task<IReadOnlyList<CreditAdminListItemDto>> GetListAsync(CreditListQuery query, CancellationToken cancellationToken);
    Task<CreditAdminListItemDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<CreditAdminListItemDto> CreateAsync(UpsertCreditRequest request, CancellationToken cancellationToken);
    Task<CreditAdminListItemDto?> UpdateAsync(Guid id, UpsertCreditRequest request, CancellationToken cancellationToken);
}
