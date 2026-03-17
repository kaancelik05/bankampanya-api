using Bankampanya.Application.Features.Credits.Dtos;
using FluentValidation;

namespace Bankampanya.Application.Features.Credits.Validators;

public sealed class UpsertCreditRequestValidator : AbstractValidator<UpsertCreditRequest>
{
    public UpsertCreditRequestValidator()
    {
        RuleFor(x => x.BankName)
            .NotEmpty()
            .MaximumLength(120);

        RuleFor(x => x.Title)
            .NotEmpty()
            .MinimumLength(8)
            .MaximumLength(200);

        RuleFor(x => x.Rate)
            .NotEmpty()
            .MaximumLength(120);

        RuleFor(x => x.AmountRange)
            .NotEmpty()
            .MaximumLength(120);

        RuleFor(x => x.DetailSummary)
            .NotEmpty()
            .MinimumLength(12)
            .MaximumLength(600);

        RuleForEach(x => x.Terms)
            .NotEmpty()
            .MaximumLength(500);

        RuleFor(x => x.Terms)
            .Must(terms => terms is not null && terms.Any(term => !string.IsNullOrWhiteSpace(term)))
            .WithMessage("En az bir kredi koşulu girilmelidir.");
    }
}
