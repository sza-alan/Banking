using Banking.API.Models.Events;
using Banking.API.Models.Exceptions;
using Banking.API.Models.Snapshots;

namespace Banking.Api.Models;

public class Account
{
    private readonly List<IDomainEvent> _uncommittedEvents = [];

    public Guid Id { get; private set; }
    public string Owner { get; private set; } = string.Empty;
    public decimal Balance { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public int Version { get; private set; }

    public IReadOnlyCollection<IDomainEvent> UncommittedEvents => _uncommittedEvents.AsReadOnly();

    private Account() { }

    public Account(string owner)
    {
        if (string.IsNullOrWhiteSpace(owner))
            throw new InvalidOperationException("O titular da conta eh obrigatorio");

        Raise(new AccountOpened(Guid.NewGuid(), owner, DateTime.UtcNow));
    }

    public void Deposit(decimal amount)
    {
        if (amount <= 0)
            throw new InvalidMoneyAmountException(amount);

        Raise(new MoneyDeposited(Id, amount, DateTime.UtcNow));
    }

    public void Withdraw(decimal amount)
    {
        if (amount <= 0)
            throw new InvalidMoneyAmountException(amount);

        if (Balance < amount)
            throw new InsufficientBalanceException(Balance, amount);

        Raise(new MoneyWithdrawn(Id, amount, DateTime.UtcNow));
    }

    private void Raise(IDomainEvent domainEvent)
    {
        Apply(domainEvent);

        _uncommittedEvents.Add(domainEvent);
    }

    public void ClearUncommittedEvents()
    {
        _uncommittedEvents.Clear();
    }

    private void Apply(IDomainEvent domainEvent)
    {
        switch (domainEvent)
        {
            case AccountOpened e:
                Id = e.AccountId;
                Owner = e.Owner;
                Balance = 0;
                CreatedAt = e.OccurredAt;
                break;

            case MoneyDeposited e:
                Balance += e.Amount;
                break;

            case MoneyWithdrawn e:
                Balance -= e.Amount;
                break;

            default:
                throw new InvalidOperationException(
                    $"Evento desconhecido: {domainEvent.GetType().Name}");
        }

        Version++;
    }

    public static Account FromHistory(IEnumerable<IDomainEvent> events)
    {
        var account = new Account();

        foreach (var domainEvent in events)
        {
            account.Apply(domainEvent);
        }

        return account;
    }

    public static Account FromSnapshot(AccountSnapshot snapshot)
    {
        var account = new Account
        {
            Id = snapshot.AggregateId,
            Owner = snapshot.Owner,
            Balance = snapshot.Balance,
            CreatedAt = snapshot.CreatedAt,
            Version = snapshot.Version
        };

        return account;
    }

    public void ApplyHistory(IEnumerable<IDomainEvent> events)
    {
        foreach (var domainEvent in events)
        {
            Apply(domainEvent);
        }
    }

    public AccountSnapshot ToSnapshot()
    {
        return new AccountSnapshot
        {
            AggregateId = Id,
            Version = Version,
            Owner = Owner,
            Balance = Balance,
            CreatedAt = CreatedAt
        };
    }
}