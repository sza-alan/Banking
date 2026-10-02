using Banking.API.Data;
using Banking.API.Models.Events;
using Banking.API.Models.ReadModels;
using Microsoft.EntityFrameworkCore;

namespace Banking.Api.Projections;

public class AccountProjection
{
    private readonly AppDbContext _context;

    public AccountProjection(AppDbContext context)
    {
        _context = context;
    }

    public async Task ApplyAsync(IDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        switch (domainEvent)
        {
            case AccountOpened e:
                await ApplyAsync(e, cancellationToken);
                break;

            case MoneyDeposited e:
                await ApplyAsync(e, cancellationToken);
                break;

            case MoneyWithdrawn e:
                await ApplyAsync(e, cancellationToken);
                break;

            default:
                throw new InvalidOperationException(
                    $"Evento desconhecido: {domainEvent.GetType().Name}");
        }
    }

    private async Task ApplyAsync(AccountOpened domainEvent, CancellationToken cancellationToken)
    {
        var readModel = new AccountReadModel
        {
            Id = domainEvent.AccountId,
            Owner = domainEvent.Owner,
            Balance = 0,
            CreatedAt = domainEvent.OccurredAt,
            LastUpdatedAt = domainEvent.OccurredAt
        };

        await _context.AccountReadModels.AddAsync(
            readModel,
            cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);
    }

    private async Task ApplyAsync(MoneyDeposited domainEvent, CancellationToken cancellationToken)
    {
        var readModel = await _context.AccountReadModels
            .FirstOrDefaultAsync(
                x => x.Id == domainEvent.AccountId,
                cancellationToken);

        if (readModel is null)
            throw new InvalidOperationException(
                $"Read model da conta {domainEvent.AccountId} não encontrado.");

        readModel.Balance += domainEvent.Amount;
        readModel.LastUpdatedAt = domainEvent.OccurredAt;

        await _context.SaveChangesAsync(cancellationToken);
    }

    private async Task ApplyAsync(MoneyWithdrawn domainEvent, CancellationToken cancellationToken)
    {
        var readModel = await _context.AccountReadModels
            .FirstOrDefaultAsync(
                x => x.Id == domainEvent.AccountId,
                cancellationToken);

        if (readModel is null)
            throw new InvalidOperationException(
                $"Read model da conta {domainEvent.AccountId} não encontrado.");

        readModel.Balance -= domainEvent.Amount;
        readModel.LastUpdatedAt = domainEvent.OccurredAt;

        await _context.SaveChangesAsync(cancellationToken);
    }
}