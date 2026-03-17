using Bankampanya.Application.Features.MobileAssistant;
using Bankampanya.Application.Features.MobileAssistant.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Bankampanya.Api.Controllers.Mobile;

[ApiController]
[Route("api/mobile/assistant/prompts")]
public class MobileAssistantController(MobileAssistantPromptQueryService mobileAssistantPromptQueryService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<MobileAssistantPromptSuggestionDto>>> GetPrompts(
        [FromQuery] string? search,
        [FromQuery] string? tone,
        CancellationToken cancellationToken)
    {
        var result = await mobileAssistantPromptQueryService.GetListAsync(new MobileAssistantPromptListQuery
        {
            Search = search,
            Tone = tone,
        }, cancellationToken);

        return Ok(result);
    }
}
