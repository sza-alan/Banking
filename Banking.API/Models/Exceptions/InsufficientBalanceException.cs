namespace Banking.API.Models.Exceptions
{
    public class InsufficientBalanceException : DomainException
    {
        public InsufficientBalanceException(
            decimal balance, 
            decimal requestedAmount) 
            : base($"Saldo insuficiente. Saldo atual: {balance:C}. Valor solicitado: {requestedAmount:C}.")
        {
        }
    }
}
