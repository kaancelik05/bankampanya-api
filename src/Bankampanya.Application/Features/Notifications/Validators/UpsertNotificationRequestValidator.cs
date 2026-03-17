using Bankampanya.Application.Features.Notifications.Dtos;
using FluentValidation;

namespace Bankampanya.Application.Features.Notifications.Validators;

public sealed class UpsertNotificationRequestValidator : AbstractValidator<UpsertNotificationRequest>
{
    public UpsertNotificationRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MinimumLength(8)
            .MaximumLength(200);

        RuleFor(x => x.Body)
            .NotEmpty()
            .MinimumLength(12)
            .MaximumLength(600);

        RuleFor(x => x.CtaLabel)
            .NotEmpty()
            .MaximumLength(120);

        RuleFor(x => x.Route)
            .NotEmpty()
            .MaximumLength(300)
            .Must(route => route.StartsWith('/'))
            .WithMessage("Route '/' ile başlamalıdır.");
    }
}
