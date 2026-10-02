using Banking.Api.Projections;
using Banking.API.Data;
using Banking.API.EventStore;
using Microsoft.EntityFrameworkCore;

namespace Banking.API.Projections
{
    public class ProjectionReplayService
    {
        private readonly AppDbContext _context;
        private readonly IEventStore _eventStore;
        private readonly AccountProjection _accountProjection;

        public ProjectionReplayService(AppDbContext context, IEventStore eventStore, AccountProjection accountProjection)
        {
            _context = context;
            _eventStore = eventStore;
            _accountProjection = accountProjection;
        }

        public async Task ReplayAsync(CancellationToken cancellationToken)
        {
            await _context.AccountReadModels.ExecuteDeleteAsync(cancellationToken);

            var events = await _eventStore.LoadAllAsync(cancellationToken);

            foreach (var domainEvent in events)
            {
                await _accountProjection.ApplyAsync(domainEvent, cancellationToken);
            }
        }
    }
}
