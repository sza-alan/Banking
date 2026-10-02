using Banking.API.Application.Commands.CreateAccount;
using FluentValidation;

namespace Banking.Api.Application.Commands.CreateAccount;

public class CreateAccountValidator : AbstractValidator<CreateAccountCommand>
{
    public CreateAccountValidator()
    {
        RuleFor(x => x.Owner)
            .NotEmpty()
            .WithMessage("O titular é obrigatório.");

        RuleFor(x => x.Owner)
            .MaximumLength(100)
            .WithMessage(
                "O titular deve possuir no máximo 100 caracteres.");
    }
}