using Bankampanya.Application.Features.Assistant.Dtos;
using FluentValidation;

namespace Bankampanya.Application.Features.Assistant.Validators;

public sealed class UpsertAssistantPromptRequestValidator : AbstractValidator<UpsertAssistantPromptRequest>
{
    public UpsertAssistantPromptRequestValidator()
    {
        RuleFor(x => x.Text)
            .NotEmpty()
            .MinimumLength(8)
            .MaximumLength(500);

        RuleFor(x => x.Tone)
            .NotEmpty()
            .MaximumLength(120);
    }
}
