namespace Banking.API.Application.Queries.GetAccountById.DTO
{
    public record AccountDto(
        Guid Id,
        string Owner,
        decimal Balance,
        DateTime CreatedAt);
}
