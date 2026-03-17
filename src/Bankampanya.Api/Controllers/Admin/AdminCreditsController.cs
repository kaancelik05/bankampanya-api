using Bankampanya.Application.Features.Credits;
using Bankampanya.Application.Features.Credits.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Bankampanya.Api.Controllers.Admin;

[ApiController]
[Route("api/admin/credits")]
public class AdminCreditsController(CreditAdminService creditAdminService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<CreditAdminListItemDto>>> GetList(
        [FromQuery] string? search,
        [FromQuery] string? type,
        [FromQuery] string? status,
        CancellationToken cancellationToken)
    {
        var result = await creditAdminService.GetListAsync(new CreditListQuery
        {
            Search = search,
            Type = type,
            Status = status,
        }, cancellationToken);

        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<CreditAdminListItemDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await creditAdminService.GetByIdAsync(id, cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<CreditAdminListItemDto>> Create(
        [FromBody] UpsertCreditRequest request,
        CancellationToken cancellationToken)
    {
        var result = await creditAdminService.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<CreditAdminListItemDto>> Update(
        Guid id,
        [FromBody] UpsertCreditRequest request,
        CancellationToken cancellationToken)
    {
        var result = await creditAdminService.UpdateAsync(id, request, cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }
}
