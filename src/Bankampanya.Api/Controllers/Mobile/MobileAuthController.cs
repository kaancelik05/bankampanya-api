using Bankampanya.Application.Features.Auth;
using Bankampanya.Application.Features.Auth.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bankampanya.Api.Controllers.Mobile;

[ApiController]
[Route("api/mobile/auth")]
public sealed class MobileAuthController(AuthService authService, AuthCurrentUserService authCurrentUserService) : ControllerBase
{
    [Authorize]
    [HttpGet("me")]
    public async Task<ActionResult<AuthUserDto>> Me(CancellationToken cancellationToken)
    {
        var result = await authCurrentUserService.GetCurrentUserAsync(cancellationToken);
        return Ok(result);
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        var result = await authService.LoginAsync(request, cancellationToken);
        return Ok(result);
    }

    [HttpPost("register")]
    public async Task<ActionResult<AuthResponseDto>> Register([FromBody] RegisterRequest request, CancellationToken cancellationToken)
    {
        var result = await authService.RegisterAsync(request, cancellationToken);
        return Ok(result);
    }

    [HttpPost("refresh")]
    public async Task<ActionResult<AuthResponseDto>> Refresh([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        var result = await authService.RefreshAsync(request, cancellationToken);
        return Ok(result);
    }

    [HttpPost("logout")]
    public async Task<ActionResult<AuthResponseDto>> Logout([FromBody] LogoutRequest request, CancellationToken cancellationToken)
    {
        var result = await authService.LogoutAsync(request, cancellationToken);
        return Ok(result);
    }

    [HttpPost("password-reset")]
    public async Task<ActionResult<AuthResponseDto>> PasswordReset([FromBody] PasswordResetRequest request, CancellationToken cancellationToken)
    {
        var result = await authService.RequestPasswordResetAsync(request, cancellationToken);
        return Ok(result);
    }
}
