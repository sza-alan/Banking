namespace Banking.API.IntegrationEvents
{
    public record AccountBalanceChangedIntegrationEvent(
        Guid EventId,
        Guid AccountId,
        decimal Amount,
        decimal CurrentBalance,
        string Operation,
        DateTime OccurredAt
    ) : IIntegrationEvent;
}
