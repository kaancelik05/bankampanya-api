using Bankampanya.Application.Features.MobileTrackingEvents.Dtos;
using FluentValidation;

namespace Bankampanya.Application.Features.MobileTrackingEvents.Validators;

public sealed class CreateTrackingEventRequestValidator : AbstractValidator<CreateTrackingEventRequest>
{
    public CreateTrackingEventRequestValidator()
    {
        RuleFor(x => x.MerchantName)
            .NotEmpty()
            .MaximumLength(160);

        RuleFor(x => x.AmountText)
            .NotEmpty()
            .MaximumLength(120);

        RuleFor(x => x.Note)
            .MaximumLength(500)
            .When(x => !string.IsNullOrWhiteSpace(x.Note));
    }
}
