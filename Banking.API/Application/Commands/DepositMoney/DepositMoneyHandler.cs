using Banking.Api.Models;
using Banking.Api.Projections;
using Banking.API.Application.Exceptions;
using Banking.API.Application.Services;
using Banking.API.EventStore;
using Banking.API.IntegrationEvents;
using MediatR;

namespace Banking.API.Application.Commands.DepositMoney
{
    public class DepositMoneyHandler : IRequestHandler<DepositMoneyCommand, Account>
    {
        private readonly AccountAggregateLoader _loader;
        private readonly IEventStore _eventStore;
        private readonly AccountProjection _projection;
        private readonly IIntegrationEventPublisher _integrationEventPublisher;

        public DepositMoneyHandler(IEventStore eventStore, AccountProjection projection, AccountAggregateLoader loader, IIntegrationEventPublisher integrationEventPublisher)
        {
            _eventStore = eventStore;
            _projection = projection;
            _loader = loader;
            _integrationEventPublisher = integrationEventPublisher;
        }

        public async Task<Account> Handle(DepositMoneyCommand request, CancellationToken cancellationToken)
        {
            var account = await _loader.LoadAsync(request.AccountId, cancellationToken);

            if (account is null)
                throw new AccountNotFoundException(
                    request.AccountId);

            var expectedVersion = account.Version;

            account.Deposit(request.Amount);

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

            var integrationEvent =
                new AccountBalanceChangedIntegrationEvent(
                    Guid.NewGuid(),
                    account.Id,
                    request.Amount,
                    account.Balance,
                    "Deposit",
                    DateTime.UtcNow);

            await _integrationEventPublisher.PublishAsync(integrationEvent, cancellationToken);

            return account;
        }
    }
}