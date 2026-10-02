using Banking.Api.Models;
using Banking.API.EventStore;
using Banking.API.Models.Snapshots;

namespace Banking.API.Application.Services
{
    public class AccountAggregateLoader
    {
        private readonly IEventStore _eventStore;
        private readonly ISnapshotStore _snapshotStore;

        public AccountAggregateLoader(IEventStore eventStore, ISnapshotStore snapshotStore)
        {
            _eventStore = eventStore;
            _snapshotStore = snapshotStore;
        }

        public async Task<Account?> LoadAsync(Guid aggregateId, CancellationToken cancellationToken)
        {
            var snapshot = await _snapshotStore.GetAsync(
                aggregateId,
                cancellationToken);

            Account? account;

            if (snapshot is not null)
            {
                account = Account.FromSnapshot(snapshot);

                var events = await _eventStore.LoadAsync(
                    aggregateId,
                    snapshot.Version,
                    cancellationToken);

                account.ApplyHistory(events);

                return account;
            }

            var fullHistory = await _eventStore.LoadAsync(
                aggregateId,
                fromVersion: 0,
                cancellationToken);

            if (fullHistory.Count == 0)
                return null;

            account = Account.FromHistory(fullHistory);

            return account;
        }
    }
}
