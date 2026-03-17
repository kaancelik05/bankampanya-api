using Bankampanya.Application.Features.Auth;
using Bankampanya.Domain.Entities;
using Bankampanya.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Bankampanya.Infrastructure.Persistence.Repositories;

public sealed class AuthUserRepository(AppDbContext dbContext) : IAuthUserRepository
{
    public Task<AppUser?> GetByEmailOrPhoneAsync(string identifier, CancellationToken cancellationToken)
    {
        var normalized = identifier.Trim().ToLowerInvariant();
        var raw = identifier.Trim();

        return dbContext.AppUsers
            .FirstOrDefaultAsync(x => x.Email.ToLower() == normalized || x.Phone == raw, cancellationToken);
    }

    public Task<AppUser?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        => dbContext.AppUsers.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public Task<bool> ExistsByEmailOrPhoneAsync(string email, string phone, CancellationToken cancellationToken)
    {
        var normalizedEmail = email.Trim().ToLowerInvariant();
        var normalizedPhone = phone.Trim();

        return dbContext.AppUsers
            .AnyAsync(x => x.Email.ToLower() == normalizedEmail || x.Phone == normalizedPhone, cancellationToken);
    }

    public async Task<AppUser> CreateAsync(AppUser user, CancellationToken cancellationToken)
    {
        dbContext.AppUsers.Add(user);
        await dbContext.SaveChangesAsync(cancellationToken);
        return user;
    }
}
