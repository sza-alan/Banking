
using Banking.API.Data;
using Microsoft.EntityFrameworkCore;

namespace Banking.API.Models.Snapshots
{
    public class SnapshotStore : ISnapshotStore
    {
        private readonly AppDbContext _context;

        public SnapshotStore(AppDbContext context)
        {
            _context = context;
        }

        public async Task<AccountSnapshot?> GetAsync(Guid aggregateId, CancellationToken cancellationToken)
        {
            return await _context.AccountSnapshots
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    x => x.AggregateId == aggregateId,
                    cancellationToken);
        }

        public async Task SaveAsync(AccountSnapshot snapshot, CancellationToken cancellationToken)
        {
            var existing = await _context.AccountSnapshots
                .FirstOrDefaultAsync(
                    x => x.AggregateId == snapshot.AggregateId,
                    cancellationToken);

            if (existing is null)
            {
                await _context.AccountSnapshots.AddAsync(
                    snapshot,
                    cancellationToken);
            }
            else
            {
                existing.Version = snapshot.Version;
                existing.Owner = snapshot.Owner;
                existing.Balance = snapshot.Balance;
                existing.CreatedAt = snapshot.CreatedAt;
            }

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
