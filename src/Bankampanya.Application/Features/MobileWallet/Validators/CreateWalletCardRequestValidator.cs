using Bankampanya.Application.Features.MobileWallet.Dtos;
using FluentValidation;

namespace Bankampanya.Application.Features.MobileWallet.Validators;

public sealed class CreateWalletCardRequestValidator : AbstractValidator<CreateWalletCardRequest>
{
    public CreateWalletCardRequestValidator()
    {
        RuleFor(x => x.BankName)
            .NotEmpty()
            .MaximumLength(120);

        RuleFor(x => x.CardType)
            .NotEmpty()
            .MaximumLength(120);

        RuleFor(x => x.CustomName)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(160);
    }
}
