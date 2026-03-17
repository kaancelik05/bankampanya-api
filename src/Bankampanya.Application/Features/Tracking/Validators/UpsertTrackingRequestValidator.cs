using Bankampanya.Application.Features.Tracking.Dtos;
using FluentValidation;

namespace Bankampanya.Application.Features.Tracking.Validators;

public sealed class UpsertTrackingRequestValidator : AbstractValidator<UpsertTrackingRequest>
{
    public UpsertTrackingRequestValidator()
    {
        RuleFor(x => x.CampaignId)
            .NotEmpty();

        RuleFor(x => x.BankName)
            .NotEmpty()
            .MaximumLength(120);

        RuleFor(x => x.Title)
            .NotEmpty()
            .MinimumLength(8)
            .MaximumLength(200);

        RuleFor(x => x.Description)
            .NotEmpty()
            .MinimumLength(12)
            .MaximumLength(500);

        RuleFor(x => x.RequirementText)
            .NotEmpty()
            .MaximumLength(500);

        RuleFor(x => x.NextActionText)
            .NotEmpty()
            .MaximumLength(500);

        RuleFor(x => x.RewardText)
            .NotEmpty()
            .MaximumLength(120);

        RuleFor(x => x.DefaultProgressTarget)
            .GreaterThan(0)
            .When(x => x.DefaultProgressTarget.HasValue);
    }
}
