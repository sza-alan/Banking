using Banking.API.Models.Events;

namespace Banking.API.EventStore
{
    public interface IEventStore
    {
        Task<IReadOnlyList<IDomainEvent>> LoadAsync(Guid aggregateId, CancellationToken cancellationToken);
        Task<IReadOnlyList<IDomainEvent>> LoadAllAsync(CancellationToken cancellationToken);
        Task AppendAsync(Guid aggregateId, int expectedVersion, IEnumerable<IDomainEvent> events, CancellationToken cancellationToken);
        Task<IReadOnlyList<IDomainEvent>> LoadAsync(Guid aggregateId, int fromVersion, CancellationToken cancellationToken);
    }
}
