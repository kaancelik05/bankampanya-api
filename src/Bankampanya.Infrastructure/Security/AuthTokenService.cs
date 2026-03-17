using System.Security.Cryptography;
using System.Text;
using Bankampanya.Application.Features.Auth;
using Bankampanya.Application.Features.Auth.Dtos;

namespace Bankampanya.Infrastructure.Security;

public sealed class AuthTokenService : IAuthTokenService
{
    public AuthSessionDto CreateSession(Guid userId, string identifier)
    {
        var expiresAt = DateTime.UtcNow.AddHours(8);
        var tokenPayload = $"{userId}:{identifier}:{expiresAt:O}";
        var accessToken = Convert.ToBase64String(Encoding.UTF8.GetBytes(tokenPayload));
        var refreshToken = Convert.ToBase64String(Encoding.UTF8.GetBytes($"refresh:{userId}:{Guid.NewGuid():N}"));

        return new AuthSessionDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            ExpiresAt = expiresAt.ToString("O"),
        };
    }

    public AuthSessionDto RefreshSession(Guid userId, string identifier, string refreshToken)
        => CreateSession(userId, identifier);

    public Guid? GetUserIdFromRefreshToken(string refreshToken)
    {
        try
        {
            var raw = Encoding.UTF8.GetString(Convert.FromBase64String(refreshToken));
            var parts = raw.Split(':', StringSplitOptions.RemoveEmptyEntries);
            return parts.Length >= 2 && Guid.TryParse(parts[1], out var userId) ? userId : null;
        }
        catch
        {
            return null;
        }
    }

    public string HashRefreshToken(string refreshToken)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(refreshToken));
        return Convert.ToHexString(bytes);
    }
}
