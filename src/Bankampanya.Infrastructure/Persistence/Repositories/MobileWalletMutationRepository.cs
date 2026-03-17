using Bankampanya.Application.Features.MobileWallet;
using Bankampanya.Application.Features.MobileWallet.Dtos;
using Bankampanya.Application.Common.Interfaces;
using Bankampanya.Domain.Entities;
using Bankampanya.Domain.Enums;
using Bankampanya.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Bankampanya.Infrastructure.Persistence.Repositories;

public sealed class MobileWalletMutationRepository(
    AppDbContext dbContext,
    ICurrentUserService currentUserService) : IMobileWalletMutationRepository
{
    public async Task<MobileWalletCardDto> CreateAsync(CreateWalletCardRequest request, CancellationToken cancellationToken)
    {
        var userId = currentUserService.GetUserId();

        var entity = new WalletCard
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            BankName = request.BankName,
            CardType = request.CardType,
            CustomName = request.CustomName,
            Status = WalletCardStatus.Active,
            CreatedAtUtc = DateTime.UtcNow,
            UpdatedAtUtc = DateTime.UtcNow,
        };

        dbContext.WalletCards.Add(entity);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Map(entity);
    }

    public async Task<MobileWalletCardDto?> UpdateStatusAsync(Guid id, UpdateWalletCardStatusRequest request, CancellationToken cancellationToken)
    {
        var userId = currentUserService.GetUserId();
        var entity = await dbContext.WalletCards.FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId, cancellationToken);
        if (entity is null)
        {
            return null;
        }

        entity.Status = request.IsActive ? WalletCardStatus.Active : WalletCardStatus.Passive;
        entity.UpdatedAtUtc = DateTime.UtcNow;

        await dbContext.SaveChangesAsync(cancellationToken);
        return Map(entity);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var userId = currentUserService.GetUserId();
        var entity = await dbContext.WalletCards.FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId, cancellationToken);
        if (entity is null)
        {
            return false;
        }

        dbContext.WalletCards.Remove(entity);
        await dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }

    private static MobileWalletCardDto Map(WalletCard entity)
        => new()
        {
            Id = entity.Id.ToString(),
            BankName = entity.BankName,
            CardType = entity.CardType,
            CustomName = entity.CustomName,
            IsActive = entity.Status == WalletCardStatus.Active,
        };
}
