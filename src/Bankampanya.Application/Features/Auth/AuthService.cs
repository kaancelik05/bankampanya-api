using Bankampanya.Application.Common.Exceptions;
using Bankampanya.Application.Features.Auth.Dtos;
using Bankampanya.Domain.Entities;

namespace Bankampanya.Application.Features.Auth;

public sealed class AuthService(
    IAuthUserRepository authUserRepository,
    IPasswordHasher passwordHasher,
    IAuthTokenService authTokenService,
    IRefreshTokenGrantRepository refreshTokenGrantRepository)
{
    private static readonly Guid DemoUserId = Guid.Parse("11111111-1111-1111-1111-111111111111");

    public async Task<AuthResponseDto> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
    {
        var identifier = request.Identifier.Trim();
        var user = await authUserRepository.GetByEmailOrPhoneAsync(identifier, cancellationToken);

        if (user is null)
        {
            if (MatchesDemoCredentials(identifier, request.Password))
            {
                return new AuthResponseDto
                {
                    Success = true,
                    User = MapUser(CreateDemoUser(identifier), identifier),
                    Session = authTokenService.CreateSession(DemoUserId, identifier),
                };
            }

            throw new UnauthorizedException("Giriş bilgileri doğrulanamadı.");
        }

        if (!passwordHasher.Verify(request.Password, user.PasswordHash))
        {
            throw new UnauthorizedException("Giriş bilgileri doğrulanamadı.");
        }

        return await BuildPersistedAuthResponseAsync(user, user.Email, cancellationToken);
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default)
    {
        var email = request.Email.Trim();
        var phone = request.Phone.Trim();

        var exists = await authUserRepository.ExistsByEmailOrPhoneAsync(email, phone, cancellationToken);
        if (exists)
        {
            throw new InvalidOperationException("Bu e-posta veya telefon ile kayıtlı bir kullanıcı zaten mevcut.");
        }

        var now = DateTime.UtcNow;
        var user = new AppUser
        {
            Id = Guid.NewGuid(),
            FullName = request.FullName.Trim(),
            Email = email,
            Phone = phone,
            PasswordHash = passwordHasher.Hash(request.Password),
            CreatedAtUtc = now,
            UpdatedAtUtc = now,
        };

        await authUserRepository.CreateAsync(user, cancellationToken);
        return await BuildPersistedAuthResponseAsync(user, user.Email, cancellationToken);
    }

    public async Task<AuthResponseDto> RefreshAsync(RefreshTokenRequest request, CancellationToken cancellationToken = default)
    {
        var userId = authTokenService.GetUserIdFromRefreshToken(request.RefreshToken);
        if (userId is null)
        {
            throw new UnauthorizedException("Oturum yenileme isteği doğrulanamadı.");
        }

        var user = await authUserRepository.GetByIdAsync(userId.Value, cancellationToken);
        if (user is null)
        {
            throw new UnauthorizedException("Oturum yenileme isteği doğrulanamadı.");
        }

        var refreshTokenHash = authTokenService.HashRefreshToken(request.RefreshToken);
        var isValid = await refreshTokenGrantRepository.IsValidAsync(user.Id, refreshTokenHash, DateTime.UtcNow, cancellationToken);
        if (!isValid)
        {
            throw new UnauthorizedException("Oturum yenileme isteği doğrulanamadı.");
        }

        await refreshTokenGrantRepository.RevokeAsync(user.Id, refreshTokenHash, DateTime.UtcNow, cancellationToken);
        return await BuildPersistedAuthResponseAsync(user, user.Email, cancellationToken);
    }

    public async Task<AuthResponseDto> LogoutAsync(LogoutRequest request, CancellationToken cancellationToken = default)
    {
        var userId = authTokenService.GetUserIdFromRefreshToken(request.RefreshToken);
        if (userId is not null)
        {
            var refreshTokenHash = authTokenService.HashRefreshToken(request.RefreshToken);
            await refreshTokenGrantRepository.RevokeAsync(userId.Value, refreshTokenHash, DateTime.UtcNow, cancellationToken);
        }

        return new AuthResponseDto
        {
            Success = true,
            Message = "Oturum kapatıldı.",
        };
    }

    public async Task<AuthResponseDto> RequestPasswordResetAsync(PasswordResetRequest request, CancellationToken cancellationToken = default)
    {
        var identifier = request.Identifier.Trim();
        var user = await authUserRepository.GetByEmailOrPhoneAsync(identifier, cancellationToken);

        var userDto = user is not null
            ? MapUser(user, user.Email)
            : new AuthUserDto
            {
                Id = DemoUserId,
                Identifier = identifier,
            };

        return new AuthResponseDto
        {
            Success = true,
            User = userDto,
            Message = $"{identifier} için sıfırlama isteği oluşturuldu.",
        };
    }

    private async Task<AuthResponseDto> BuildPersistedAuthResponseAsync(AppUser user, string identifier, CancellationToken cancellationToken)
    {
        var session = authTokenService.CreateSession(user.Id, identifier);
        var refreshTokenHash = authTokenService.HashRefreshToken(session.RefreshToken);
        var expiresAtUtc = DateTime.Parse(session.ExpiresAt, null, System.Globalization.DateTimeStyles.RoundtripKind).ToUniversalTime();
        await refreshTokenGrantRepository.StoreAsync(user.Id, refreshTokenHash, expiresAtUtc, cancellationToken);

        return new AuthResponseDto
        {
            Success = true,
            User = MapUser(user, identifier),
            Session = session,
        };
    }

    private static bool MatchesDemoCredentials(string identifier, string password)
        => (identifier.Equals("demo@bankampanya.com", StringComparison.OrdinalIgnoreCase) || identifier == "+90 555 000 00 00")
           && password == "123456";

    private static AppUser CreateDemoUser(string identifier)
        => new()
        {
            Id = DemoUserId,
            FullName = "Demo Kullanıcı",
            Email = identifier.Contains('@') ? identifier : "demo@bankampanya.com",
            Phone = identifier.Contains('@') ? "+90 555 000 00 00" : identifier,
            PasswordHash = string.Empty,
        };

    private static AuthUserDto MapUser(AppUser user, string identifier)
        => new()
        {
            Id = user.Id,
            Identifier = identifier,
            FullName = user.FullName,
            Email = user.Email,
            Phone = user.Phone,
        };
}
