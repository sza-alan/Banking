using Banking.Api.Models;
using Banking.Api.Projections;
using Banking.API.EventStore;
using MediatR;

namespace Banking.API.Application.Commands.CreateAccount
{
    public class CreateAccountHandler : IRequestHandler<CreateAccountCommand, Account>
    {
        private readonly IEventStore _eventStore;
        private readonly AccountProjection _projection;

        public CreateAccountHandler(IEventStore eventStore, AccountProjection projection)
        {
            _eventStore = eventStore;
            _projection = projection;
        }

        public async Task<Account> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
        {
            var account = new Account(request.Owner);

            await _eventStore.AppendAsync(
                account.Id,
                expectedVersion: 0,
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
