using Banking.Api.Models;
using Banking.Api.Projections;
using Banking.API.Application.Exceptions;
using Banking.API.Application.Services;
using Banking.API.EventStore;
using MediatR;

namespace Banking.API.Application.Commands.WithdrawMoney
{
    public class WithdrawMoneyHandler : IRequestHandler<WithdrawMoneyCommand, Account>
    {
        private readonly AccountAggregateLoader _loader;
        private readonly IEventStore _eventStore;
        private readonly AccountProjection _projection;

        public WithdrawMoneyHandler(IEventStore eventStore, AccountProjection projection, AccountAggregateLoader loader)
        {
            _eventStore = eventStore;
            _projection = projection;
            _loader = loader;
        }

        public async Task<Account> Handle(WithdrawMoneyCommand request, CancellationToken cancellationToken)
        {
            var account = await _loader.LoadAsync(request.AccountId, cancellationToken);

            if (account is null)
                throw new AccountNotFoundException(request.AccountId);

            var expectedVersion = account.Version;

            account.Withdraw(request.Amount);

            await _eventStore.AppendAsync(
                account.Id, 
                expectedVersion, 
                account.UncommittedEvents, 
                cancellationToken);

            foreach (var domainEvent in account.UncommittedEvents)
            {
                await _projection.ApplyAsync(
                    domainEvent,
                    cancellationToken);
            }

            account.ClearUncommittedEvents();

            return account;
        }
    }
}
