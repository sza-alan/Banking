namespace Banking.API.Models.Events
{
    public record AccountOpened(Guid AccountId, string Owner, DateTime OccurredAt) : IDomainEvent;
}
