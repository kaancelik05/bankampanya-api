using Bankampanya.Application.Features.Auth;
using Bankampanya.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Bankampanya.Infrastructure.Persistence.Repositories;

public sealed class RefreshTokenGrantRepository(AppDbContext dbContext) : IRefreshTokenGrantRepository
{
    public async Task StoreAsync(Guid userId, string tokenHash, DateTime expiresAtUtc, CancellationToken cancellationToken)
    {
        dbContext.RefreshTokenGrants.Add(new Bankampanya.Domain.Entities.RefreshTokenGrant
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            TokenHash = tokenHash,
            ExpiresAtUtc = expiresAtUtc,
            CreatedAtUtc = DateTime.UtcNow,
        });

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public Task<bool> IsValidAsync(Guid userId, string tokenHash, DateTime nowUtc, CancellationToken cancellationToken)
        => dbContext.RefreshTokenGrants.AnyAsync(
            x => x.UserId == userId
                 && x.TokenHash == tokenHash
                 && x.RevokedAtUtc == null
                 && x.ExpiresAtUtc > nowUtc,
            cancellationToken);

    public async Task RevokeAsync(Guid userId, string tokenHash, DateTime revokedAtUtc, CancellationToken cancellationToken)
    {
        var grant = await dbContext.RefreshTokenGrants
            .FirstOrDefaultAsync(x => x.UserId == userId && x.TokenHash == tokenHash && x.RevokedAtUtc == null, cancellationToken);

        if (grant is null)
        {
            return;
        }

        grant.RevokedAtUtc = revokedAtUtc;
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
