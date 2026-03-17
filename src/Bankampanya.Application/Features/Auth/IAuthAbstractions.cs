using Bankampanya.Application.Features.Auth.Dtos;
using Bankampanya.Domain.Entities;

namespace Bankampanya.Application.Features.Auth;

public interface IAuthUserRepository
{
    Task<AppUser?> GetByEmailOrPhoneAsync(string identifier, CancellationToken cancellationToken);
    Task<AppUser?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<bool> ExistsByEmailOrPhoneAsync(string email, string phone, CancellationToken cancellationToken);
    Task<AppUser> CreateAsync(AppUser user, CancellationToken cancellationToken);
}

public interface IPasswordHasher
{
    string Hash(string password);
    bool Verify(string password, string passwordHash);
}

public interface IRefreshTokenGrantRepository
{
    Task StoreAsync(Guid userId, string tokenHash, DateTime expiresAtUtc, CancellationToken cancellationToken);
    Task<bool> IsValidAsync(Guid userId, string tokenHash, DateTime nowUtc, CancellationToken cancellationToken);
    Task RevokeAsync(Guid userId, string tokenHash, DateTime revokedAtUtc, CancellationToken cancellationToken);
}

public interface IAuthTokenService
{
    AuthSessionDto CreateSession(Guid userId, string identifier);
    AuthSessionDto RefreshSession(Guid userId, string identifier, string refreshToken);
    Guid? GetUserIdFromRefreshToken(string refreshToken);
    string HashRefreshToken(string refreshToken);
}
