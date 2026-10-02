using Banking.Api.Models;
using MediatR;

namespace Banking.API.Application.Commands.WithdrawMoney
{
    public record WithdrawMoneyCommand(Guid AccountId, decimal Amount) : IRequest<Account>;
}
