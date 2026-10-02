using Banking.API.Application.Commands.WithdrawMoney;
using FluentValidation;

namespace Banking.Api.Application.Commands.WithdrawMoney;

public class WithdrawMoneyValidator : AbstractValidator<WithdrawMoneyCommand>
{
    public WithdrawMoneyValidator()
    {
        RuleFor(x => x.AccountId)
            .NotEmpty()
            .WithMessage("AccountId é obrigatório.");

        RuleFor(x => x.Amount)
            .GreaterThan(0)
            .WithMessage("O valor do saque deve ser maior que zero.");
    }
}