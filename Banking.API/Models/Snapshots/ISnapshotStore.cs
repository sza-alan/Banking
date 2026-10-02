namespace Banking.API.Models.Snapshots
{
    public interface ISnapshotStore
    {
        Task<AccountSnapshot?> GetAsync(Guid aggregateId, CancellationToken cancellationToken);
        Task SaveAsync(AccountSnapshot snapshot, CancellationToken cancellationToken);
    }
}
