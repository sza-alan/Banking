using Banking.Api.Models;

namespace Banking.API.Repositories.Interfaces
{
    public interface IAccountRepository
    {
        Task<Account?> GetByIdAsync(Guid id);
        Task AddAsync(Account account);
        Task SaveChangesAsync();
    }
}
