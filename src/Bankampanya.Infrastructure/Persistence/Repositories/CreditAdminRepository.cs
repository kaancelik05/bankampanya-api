using Bankampanya.Application.Features.Credits;
using Bankampanya.Application.Features.Credits.Dtos;
using Bankampanya.Domain.Entities;
using Bankampanya.Domain.Enums;
using Bankampanya.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Bankampanya.Infrastructure.Persistence.Repositories;

public sealed class CreditAdminRepository(AppDbContext dbContext) : ICreditAdminRepository
{
    public async Task<IReadOnlyList<CreditAdminListItemDto>> GetListAsync(CreditListQuery query, CancellationToken cancellationToken)
    {
        var creditsQuery = dbContext.CreditOffers
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.Status) && Enum.TryParse<PublishStatus>(query.Status, true, out var status))
        {
            creditsQuery = creditsQuery.Where(x => x.Status == status);
        }

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

        return credits.Select(MapToDto).ToList();
    }

    public async Task<CreditAdminListItemDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var credit = await dbContext.CreditOffers
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        return credit is null ? null : MapToDto(credit);
    }

    public async Task<CreditAdminListItemDto> CreateAsync(UpsertCreditRequest request, CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;
        var credit = new CreditOffer
        {
            Id = Guid.NewGuid(),
            BankName = request.BankName.Trim(),
            Title = request.Title.Trim(),
            Type = request.Type,
            Subtype = request.Subtype,
            Rate = request.Rate.Trim(),
            AmountRange = request.AmountRange.Trim(),
            DetailSummary = request.DetailSummary.Trim(),
            Terms = request.Terms.Where(term => !string.IsNullOrWhiteSpace(term)).Select(term => term.Trim()).ToList(),
            Status = request.Status,
            CreatedAtUtc = now,
            UpdatedAtUtc = now,
            PublishedAtUtc = request.Status == PublishStatus.Live ? now : null,
        };

        dbContext.CreditOffers.Add(credit);
        await dbContext.SaveChangesAsync(cancellationToken);

        return MapToDto(credit);
    }

    public async Task<CreditAdminListItemDto?> UpdateAsync(Guid id, UpsertCreditRequest request, CancellationToken cancellationToken)
    {
        var credit = await dbContext.CreditOffers
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (credit is null)
        {
            return null;
        }

        var now = DateTime.UtcNow;
        credit.BankName = request.BankName.Trim();
        credit.Title = request.Title.Trim();
        credit.Type = request.Type;
        credit.Subtype = request.Subtype;
        credit.Rate = request.Rate.Trim();
        credit.AmountRange = request.AmountRange.Trim();
        credit.DetailSummary = request.DetailSummary.Trim();
        credit.Terms = request.Terms.Where(term => !string.IsNullOrWhiteSpace(term)).Select(term => term.Trim()).ToList();
        credit.Status = request.Status;
        credit.UpdatedAtUtc = now;
        credit.PublishedAtUtc ??= request.Status == PublishStatus.Live ? now : null;

        await dbContext.SaveChangesAsync(cancellationToken);

        return MapToDto(credit);
    }

    private static CreditAdminListItemDto MapToDto(CreditOffer credit)
    {
        return new CreditAdminListItemDto
        {
            Id = credit.Id,
            BankName = credit.BankName,
            Title = credit.Title,
            Type = credit.Type,
            Subtype = credit.Subtype,
            Rate = credit.Rate,
            AmountRange = credit.AmountRange,
            DetailSummary = credit.DetailSummary,
            Terms = credit.Terms.ToArray(),
            Status = credit.Status,
            CreatedAtUtc = credit.CreatedAtUtc,
            UpdatedAtUtc = credit.UpdatedAtUtc,
            PublishedAtUtc = credit.PublishedAtUtc,
        };
    }
}
