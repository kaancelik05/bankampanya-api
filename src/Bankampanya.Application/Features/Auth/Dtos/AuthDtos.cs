namespace Bankampanya.Application.Features.Auth.Dtos;

public sealed class LoginRequest
{
    public string Identifier { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
}

public sealed class RegisterRequest
{
    public string FullName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string Phone { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
}

public sealed class PasswordResetRequest
{
    public string Identifier { get; init; } = string.Empty;
}

public sealed class RefreshTokenRequest
{
    public string RefreshToken { get; init; } = string.Empty;
}

public sealed class LogoutRequest
{
    public string RefreshToken { get; init; } = string.Empty;
}

public sealed class AuthUserDto
{
    public Guid Id { get; init; }
    public string? Identifier { get; init; }
    public string? FullName { get; init; }
    public string? Email { get; init; }
    public string? Phone { get; init; }
}

public sealed class AuthSessionDto
{
    public string AccessToken { get; init; } = string.Empty;
    public string RefreshToken { get; init; } = string.Empty;
    public string ExpiresAt { get; init; } = string.Empty;
}

public sealed class AuthResponseDto
{
    public bool Success { get; init; }
    public AuthUserDto User { get; init; } = new();
    public AuthSessionDto? Session { get; init; }
    public string? Message { get; init; }
}
