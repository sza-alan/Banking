using FluentValidation;

namespace Banking.API.Application.Commands.DepositMoney
{
    public class DepositMoneyValidator : AbstractValidator<DepositMoneyCommand>
    {
        public DepositMoneyValidator()
        {
            RuleFor(x => x.AccountId)
                .NotEmpty()
                .WithMessage("AccountId é obrigatório.");

            RuleFor(x => x.Amount)
                .GreaterThan(0)
                .WithMessage("O valor do depósito deve ser maior que zero.");
        }
    }
}
