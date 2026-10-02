using Banking.API.Application.Exceptions;
using Banking.API.Application.Queries.GetAccountById.DTO;
using Banking.API.Data;
using Banking.API.Repositories.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Banking.API.Application.Queries.GetAccountById
{
    public class GetAccountByIdHandler : IRequestHandler<GetAccountByIdQuery, AccountDto>
    {
        private readonly AppDbContext _context;

        public GetAccountByIdHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<AccountDto> Handle(GetAccountByIdQuery request, CancellationToken cancellationToken)
        {
            var account = await _context.AccountReadModels
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == request.AccountId, cancellationToken);

            if (account is null)
                throw new AccountNotFoundException(request.AccountId);

            return new AccountDto(
                account.Id,
                account.Owner,
                account.Balance,
                account.CreatedAt);
        }
    }
}
