using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Bankampanya.Api.Configuration;
using Bankampanya.Application.Features.Auth;
using Bankampanya.Application.Features.Auth.Dtos;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Bankampanya.Api.Services;

public sealed class JwtAuthTokenService(IOptions<JwtOptions> jwtOptions) : IAuthTokenService
{
    private readonly JwtOptions _jwtOptions = jwtOptions.Value;

    public AuthSessionDto CreateSession(Guid userId, string identifier)
        => BuildSession(userId, identifier);

    public AuthSessionDto RefreshSession(Guid userId, string identifier, string refreshToken)
        => BuildSession(userId, identifier);

    public Guid? GetUserIdFromRefreshToken(string refreshToken)
    {
        var handler = new JwtSecurityTokenHandler();
        if (!handler.CanReadToken(refreshToken))
        {
            return null;
        }

        var token = handler.ReadJwtToken(refreshToken);
        var userIdValue = token.Claims.FirstOrDefault(claim => claim.Type == JwtRegisteredClaimNames.Sub || claim.Type == "user_id")?.Value;
        return Guid.TryParse(userIdValue, out var userId) ? userId : null;
    }

    public string HashRefreshToken(string refreshToken)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(refreshToken));
        return Convert.ToHexString(bytes);
    }

    private AuthSessionDto BuildSession(Guid userId, string identifier)
    {
        var expiresAt = DateTime.UtcNow.AddMinutes(_jwtOptions.AccessTokenLifetimeMinutes);
        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SigningKey));
        var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, identifier),
            new Claim("user_id", userId.ToString()),
        };

        var token = new JwtSecurityToken(
            issuer: _jwtOptions.Issuer,
            audience: _jwtOptions.Audience,
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: expiresAt,
            signingCredentials: credentials);

        var handler = new JwtSecurityTokenHandler();

        return new AuthSessionDto
        {
            AccessToken = handler.WriteToken(token),
            RefreshToken = CreateRefreshToken(userId, identifier),
            ExpiresAt = expiresAt.ToString("O"),
        };
    }

    private string CreateRefreshToken(Guid userId, string identifier)
    {
        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SigningKey));
        var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
        var expiresAt = DateTime.UtcNow.AddDays(14);
        var refreshToken = new JwtSecurityToken(
            issuer: _jwtOptions.Issuer,
            audience: _jwtOptions.Audience,
            claims: new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, identifier),
                new Claim("user_id", userId.ToString()),
                new Claim("typ", "refresh"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
            },
            notBefore: DateTime.UtcNow,
            expires: expiresAt,
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(refreshToken);
    }
}
