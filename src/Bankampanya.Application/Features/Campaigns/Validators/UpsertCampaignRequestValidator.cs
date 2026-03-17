using Bankampanya.Application.Features.Campaigns.Dtos;
using FluentValidation;

namespace Bankampanya.Application.Features.Campaigns.Validators;

public sealed class UpsertCampaignRequestValidator : AbstractValidator<UpsertCampaignRequest>
{
    public UpsertCampaignRequestValidator()
    {
        RuleFor(x => x.BankName)
            .NotEmpty()
            .MaximumLength(120);

        RuleFor(x => x.Category)
            .NotEmpty()
            .MaximumLength(120);

        RuleFor(x => x.Title)
            .NotEmpty()
            .MinimumLength(8)
            .MaximumLength(200);

        RuleFor(x => x.ShortDescription)
            .NotEmpty()
            .MinimumLength(12)
            .MaximumLength(500);

        RuleFor(x => x.RewardText)
            .NotEmpty()
            .MaximumLength(120);

        RuleFor(x => x.DeadlineText)
            .NotEmpty()
            .MaximumLength(120);

        RuleFor(x => x.ValidDateRangeLabel)
            .NotEmpty()
            .MaximumLength(120);

        RuleFor(x => x.NextActionText)
            .MaximumLength(500)
            .When(x => !string.IsNullOrWhiteSpace(x.NextActionText));

        RuleFor(x => x.ProgressTarget)
            .NotNull()
            .GreaterThan(0)
            .When(x => x.IsProgressive)
            .WithMessage("Progressive kampanyalarda ProgressTarget zorunludur.");

        RuleFor(x => x.NextActionText)
            .NotEmpty()
            .When(x => x.IsProgressive)
            .WithMessage("Progressive kampanyalarda NextActionText zorunludur.");

        RuleForEach(x => x.Terms)
            .NotEmpty()
            .MaximumLength(500);

        RuleFor(x => x.Terms)
            .Must(terms => terms is not null && terms.Any(term => !string.IsNullOrWhiteSpace(term)))
            .WithMessage("En az bir kampanya koşulu girilmelidir.");

        RuleFor(x => x)
            .Must(x => !x.ValidFromUtc.HasValue || !x.ValidToUtc.HasValue || x.ValidFromUtc <= x.ValidToUtc)
            .WithMessage("ValidFromUtc, ValidToUtc tarihinden büyük olamaz.");
    }
}
