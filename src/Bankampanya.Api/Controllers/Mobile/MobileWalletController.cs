using Bankampanya.Application.Features.MobileWallet;
using Bankampanya.Application.Features.MobileWallet.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Bankampanya.Api.Controllers.Mobile;

[ApiController]
[Route("api/mobile/wallet")]
public class MobileWalletController(
    MobileWalletQueryService mobileWalletQueryService,
    MobileWalletMutationService mobileWalletMutationService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<MobileWalletDto>> Get(CancellationToken cancellationToken)
    {
        var result = await mobileWalletQueryService.GetAsync(cancellationToken);
        return Ok(result);
    }

    [HttpPost("cards")]
    public async Task<ActionResult<MobileWalletCardDto>> Create(
        [FromBody] CreateWalletCardRequest request,
        CancellationToken cancellationToken)
    {
        var result = await mobileWalletMutationService.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(Get), null, result);
    }

    [HttpPatch("cards/{id:guid}/status")]
    public async Task<ActionResult<MobileWalletCardDto>> UpdateStatus(
        Guid id,
        [FromBody] UpdateWalletCardStatusRequest request,
        CancellationToken cancellationToken)
    {
        var result = await mobileWalletMutationService.UpdateStatusAsync(id, request, cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpDelete("cards/{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var deleted = await mobileWalletMutationService.DeleteAsync(id, cancellationToken);
        return deleted ? NoContent() : NotFound();
    }
}
