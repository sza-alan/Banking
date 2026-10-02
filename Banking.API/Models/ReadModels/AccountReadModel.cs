namespace Banking.API.Models.ReadModels
{
    public class AccountReadModel
    {
        public Guid Id { get; set; }
        public string Owner { get; set; } = string.Empty;
        public decimal Balance { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdatedAt { get; set; }
    }
}
