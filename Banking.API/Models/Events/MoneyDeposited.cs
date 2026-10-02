namespace Banking.API.Models.Events
{
    public record MoneyDeposited(Guid AccountId, decimal Amount, DateTime OccurredAt) : IDomainEvent;
}
