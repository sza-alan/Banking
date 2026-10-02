using Banking.Api.Models;
using MediatR;

namespace Banking.API.Application.Commands.CreateAccount
{
    public record CreateAccountCommand(string Owner) : IRequest<Account>;
}
