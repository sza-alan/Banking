namespace Banking.API.Application.Exceptions
{
    public class AccountNotFoundException : Exception
    {
        public AccountNotFoundException(Guid accountId) : base($"Conta {accountId} não encontrada.") { }
    }
}
