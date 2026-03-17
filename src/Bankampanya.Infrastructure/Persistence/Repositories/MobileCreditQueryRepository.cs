using Bankampanya.Application.Features.MobileCredits;
using Bankampanya.Application.Features.MobileCredits.Dtos;
using Bankampanya.Domain.Entities;
using Bankampanya.Domain.Enums;
using Bankampanya.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Bankampanya.Infrastructure.Persistence.Repositories;

public sealed class MobileCreditQueryRepository(AppDbContext dbContext) : IMobileCreditQueryRepository
{
    public async Task<IReadOnlyList<MobileCreditListItemDto>> GetListAsync(MobileCreditListQuery query, CancellationToken cancellationToken)
    {
        var creditsQuery = dbContext.CreditOffers
            .AsNoTracking()
            .Where(x => x.Status == PublishStatus.Live)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.Type) && Enum.TryParse<CreditOfferType>(query.Type, true, out var type))
        {
            creditsQuery = creditsQuery.Where(x => x.Type == type);
        }

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var search = query.Search.Trim();
            creditsQuery = creditsQuery.Where(x =>
                x.Title.Contains(search) ||
                x.BankName.Contains(search) ||
                x.DetailSummary.Contains(search));
        }

        var credits = await creditsQuery
            .OrderByDescending(x => x.UpdatedAtUtc)
            .ToListAsync(cancellationToken);

        return credits.Select(MapListItem).ToList();
    }

    public async Task<MobileCreditDetailDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var credit = await dbContext.CreditOffers
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id && x.Status == PublishStatus.Live, cancellationToken);

        return credit is null ? null : MapDetail(credit);
    }

    private static MobileCreditListItemDto MapListItem(CreditOffer credit)
    {
        return new MobileCreditListItemDto
        {
            Id = credit.Id,
            Title = credit.Title,
            BankName = credit.BankName,
            Rate = credit.Rate,
            AmountRange = credit.AmountRange,
            Type = credit.Type,
            Subtype = credit.Subtype,
            DetailSummary = credit.DetailSummary,
        };
    }

    private static MobileCreditDetailDto MapDetail(CreditOffer credit)
    {
        return new MobileCreditDetailDto
        {
            Id = credit.Id,
            Title = credit.Title,
            BankName = credit.BankName,
            Rate = credit.Rate,
            AmountRange = credit.AmountRange,
            Type = credit.Type,
            Subtype = credit.Subtype,
            DetailSummary = credit.DetailSummary,
            Terms = credit.Terms.ToArray(),
        };
    }
}
