using Banking.Api.Models;
using MediatR;

namespace Banking.API.Application.Commands.DepositMoney
{
    public record DepositMoneyCommand(Guid AccountId, decimal Amount) : IRequest<Account>;
}
