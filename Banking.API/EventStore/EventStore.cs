using Banking.API.Data;
using Banking.API.Models;
using Banking.API.Models.Events;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Banking.API.EventStore
{
    public class EventStore : IEventStore
    {
        private readonly AppDbContext _context;

        public EventStore(AppDbContext context)
        {
            _context = context;
        }

        public async Task AppendAsync(Guid aggregateId, int expectedVersion, IEnumerable<IDomainEvent> events, CancellationToken cancellationToken)
        {
            var currentVersion = await _context.StoredEvents
                .Where(x => x.AggregateId == aggregateId)
                .Select(x => (int?)x.Version)
                .MaxAsync(cancellationToken)
                ?? 0;

            if (currentVersion != expectedVersion)
            {
                throw new InvalidOperationException(
                    $"Conflito de concorrência. Versão esperada: {expectedVersion}. Versão atual: {currentVersion}.");
            }

            var nextVersion = expectedVersion;

            foreach (var domainEvent in events)
            {
                nextVersion++;

                var storedEvent = new StoredEvent
                {
                    Id = Guid.NewGuid(),
                    AggregateId = aggregateId,
                    Version = nextVersion,
                    EventType = domainEvent.GetType().Name,
                    Data = JsonSerializer.Serialize(domainEvent, domainEvent.GetType()),
                    OccurredAt = domainEvent.OccurredAt,
                };

                await _context.StoredEvents.AddAsync(storedEvent, cancellationToken);
            }

            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<IDomainEvent>> LoadAsync(Guid aggregateId, CancellationToken cancellationToken)
        {
            var storedEvent = await _context.StoredEvents
                .AsNoTracking()
                .Where(x => x.AggregateId == aggregateId)
                .OrderBy(x => x.Version)
                .ToListAsync(cancellationToken);

            return storedEvent.Select(DeserializeEvent).ToList();
        }

        public async Task<IReadOnlyList<IDomainEvent>> LoadAllAsync(CancellationToken cancellationToken)
        {
            var storedEvents = await _context.StoredEvents
                .AsNoTracking()
                .OrderBy(x => x.AggregateId)
                .ThenBy(x => x.Version)
                .ToListAsync(cancellationToken);

            return storedEvents.Select(DeserializeEvent).ToList();
        }

        public async Task<IReadOnlyList<IDomainEvent>> LoadAsync(Guid aggregateId, int fromVersion, CancellationToken cancellationToken)
        {
            var storedEvents = await _context.StoredEvents
                .AsNoTracking()
                .Where(x =>
                    x.AggregateId == aggregateId &&
                    x.Version > fromVersion)
                .OrderBy(x => x.Version)
                .ToListAsync(cancellationToken);

            return storedEvents
                .Select(DeserializeEvent)
                .ToList();
        }

        private static IDomainEvent DeserializeEvent(StoredEvent storedEvent)
        {
            return storedEvent.EventType switch
            {
                nameof(AccountOpened) =>
                    JsonSerializer.Deserialize<AccountOpened>(
                        storedEvent.Data)!,

                nameof(MoneyDeposited) =>
                    JsonSerializer.Deserialize<MoneyDeposited>(
                        storedEvent.Data)!,

                nameof(MoneyWithdrawn) =>
                    JsonSerializer.Deserialize<MoneyWithdrawn>(
                        storedEvent.Data)!,

                _ => throw new InvalidOperationException(
                    $"Tipo de evento desconhecido: {storedEvent.EventType}")
            };
        }
    }
}
