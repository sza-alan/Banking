namespace Banking.API.IntegrationEvents
{
    public interface IIntegrationEvent
    {
        Guid EventId { get; }
        DateTime OccurredAt { get; }
    }
}
