namespace Banking.API.Models.Events
{
    public interface IDomainEvent
    {
        DateTime OccurredAt { get; }
    }
}
