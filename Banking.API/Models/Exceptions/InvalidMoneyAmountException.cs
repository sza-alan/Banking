namespace Banking.API.Models.Exceptions
{
    public class InvalidMoneyAmountException : DomainException
    {
        public InvalidMoneyAmountException(decimal amount) 
            : base($"O valor informado deve ser maior que zero. Valor recebido: {amount}.")
        {
        }
    }
}
