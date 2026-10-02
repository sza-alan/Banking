using Banking.API.Application.Queries.GetAccountById.DTO;
using MediatR;

namespace Banking.API.Application.Queries.GetAccountById
{
    public record GetAccountByIdQuery(Guid AccountId) : IRequest<AccountDto>;
}
