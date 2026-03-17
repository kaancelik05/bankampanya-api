using Bankampanya.Application.Features.Assistant;
using Bankampanya.Application.Features.Assistant.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Bankampanya.Api.Controllers.Admin;

[ApiController]
[Route("api/admin/assistant-prompts")]
public class AdminAssistantPromptsController(AssistantPromptAdminService assistantPromptAdminService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<AssistantPromptAdminListItemDto>>> GetList(
        [FromQuery] string? search,
        [FromQuery] string? tone,
        [FromQuery] string? status,
        CancellationToken cancellationToken)
    {
        var result = await assistantPromptAdminService.GetListAsync(new AssistantPromptListQuery
        {
            Search = search,
            Tone = tone,
            Status = status,
        }, cancellationToken);

        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<AssistantPromptAdminListItemDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await assistantPromptAdminService.GetByIdAsync(id, cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<AssistantPromptAdminListItemDto>> Create(
        [FromBody] UpsertAssistantPromptRequest request,
        CancellationToken cancellationToken)
    {
        var result = await assistantPromptAdminService.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<AssistantPromptAdminListItemDto>> Update(
        Guid id,
        [FromBody] UpsertAssistantPromptRequest request,
        CancellationToken cancellationToken)
    {
        var result = await assistantPromptAdminService.UpdateAsync(id, request, cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }
}
