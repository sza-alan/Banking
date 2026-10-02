using Banking.API.Application.Commands.CreateAccount;
using Banking.API.Application.Commands.DepositMoney;
using Banking.API.Application.Commands.WithdrawMoney;
using Banking.API.Application.Queries.GetAccountById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Banking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly ISender _sender;

        public AccountsController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateAccountRequest request)
        {
            var command = new CreateAccountCommand(request.Owner);

            var account = await _sender.Send(command);

            return CreatedAtAction(nameof(GetById), new { id = account.Id }, account);
        }

        [HttpPost("{id:guid}/deposit")]
        public async Task<IActionResult> Deposit(Guid id, MoneyRequest request)
        {
            var command = new DepositMoneyCommand(id, request.Amount);

            var account = await _sender.Send(command);

            return Ok(account);
        }

        [HttpPost("{id:guid}/withdraw")]
        public async Task<IActionResult> Withdraw(Guid id, MoneyRequest request)
        {
            var command = new WithdrawMoneyCommand(id, request.Amount);

            var account = await _sender.Send(command);

            return Ok(account);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var query = new GetAccountByIdQuery(id);

            var account = await _sender.Send(query);

            return Ok(account);
        }

        public record CreateAccountRequest(string Owner);
        public record MoneyRequest(decimal Amount);
    }
}
