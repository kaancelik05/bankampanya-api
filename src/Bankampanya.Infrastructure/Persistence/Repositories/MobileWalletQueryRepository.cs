using Bankampanya.Application.Common.Interfaces;
using Bankampanya.Application.Features.MobileWallet;
using Bankampanya.Application.Features.MobileWallet.Dtos;
using Bankampanya.Domain.Enums;
using Bankampanya.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Bankampanya.Infrastructure.Persistence.Repositories;

public sealed class MobileWalletQueryRepository(
    AppDbContext dbContext,
    ICurrentUserService currentUserService) : IMobileWalletQueryRepository
{
    public async Task<MobileWalletDto> GetAsync(CancellationToken cancellationToken)
    {
        var userId = currentUserService.GetUserId();

        var walletCards = await dbContext.WalletCards
            .AsNoTracking()
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.Status)
            .ThenBy(x => x.BankName)
            .Select(card => new MobileWalletCardDto
            {
                Id = card.Id.ToString(),
                BankName = card.BankName,
                CardType = card.CardType,
                CustomName = card.CustomName,
                IsActive = card.Status == WalletCardStatus.Active,
            })
            .ToListAsync(cancellationToken);

        if (walletCards.Count > 0)
        {
            return new MobileWalletDto
            {
                Cards = walletCards,
            };
        }

        var liveBanks = await dbContext.Campaigns
            .AsNoTracking()
            .Where(x => x.Status == PublishStatus.Live)
            .Select(x => x.BankName)
            .Distinct()
            .OrderBy(x => x)
            .Take(4)
            .ToListAsync(cancellationToken);

        var cards = liveBanks.Select((bankName, index) => new MobileWalletCardDto
        {
            Id = $"w-{index + 1}",
            BankName = bankName,
            CardType = index % 2 == 0 ? "Kredi Kartı" : "Banka Kartı",
            CustomName = BuildCardName(bankName, index),
            IsActive = index % 2 == 0,
        }).ToArray();

        return new MobileWalletDto
        {
            Cards = cards,
        };
    }

    private static string BuildCardName(string bankName, int index)
    {
        return bankName switch
        {
            "Akbank" => "Axess Platinum",
            "Yapı Kredi" => "World Everyday",
            "Garanti BBVA" => "Bonus Gold",
            "İş Bankası" => "Maximum Genç",
            _ => $"{bankName} Kart {index + 1}",
        };
    }
}
