using Bankampanya.Application.Common.Interfaces;
using Bankampanya.Application.Features.Auth.Dtos;

namespace Bankampanya.Application.Features.Auth;

public sealed class AuthCurrentUserService(
    ICurrentUserService currentUserService,
    IAuthUserRepository authUserRepository)
{
    public async Task<AuthUserDto> GetCurrentUserAsync(CancellationToken cancellationToken = default)
    {
        var userId = currentUserService.GetUserId();
        var user = await authUserRepository.GetByIdAsync(userId, cancellationToken);

        if (user is not null)
        {
            return new AuthUserDto
            {
                Id = user.Id,
                Identifier = user.Email,
                FullName = user.FullName,
                Email = user.Email,
                Phone = user.Phone,
            };
        }

        return new AuthUserDto
        {
            Id = userId,
            Identifier = "demo@bankampanya.com",
            FullName = "Demo Kullanıcı",
            Email = "demo@bankampanya.com",
            Phone = "+90 555 000 00 00",
        };
    }
}
