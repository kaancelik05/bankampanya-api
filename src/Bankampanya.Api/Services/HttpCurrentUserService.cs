using System.Security.Claims;
using Bankampanya.Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Bankampanya.Api.Services;

public sealed class HttpCurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
{
    private static readonly Guid DemoUserId = Guid.Parse("11111111-1111-1111-1111-111111111111");

    public Guid GetUserId()
    {
        var user = httpContextAccessor.HttpContext?.User;
        var candidate = user?.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? user?.FindFirstValue("sub")
            ?? user?.FindFirstValue("user_id")
            ?? httpContextAccessor.HttpContext?.Request.Headers["X-Demo-User-Id"].FirstOrDefault();

        return Guid.TryParse(candidate, out var parsed) ? parsed : DemoUserId;
    }
}
