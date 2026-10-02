namespace Banking.API.Models.Events
{
    public record MoneyWithdrawn(Guid AccountId, decimal Amount, DateTime OccurredAt) : IDomainEvent;
}
