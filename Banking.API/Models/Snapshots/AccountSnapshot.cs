namespace Banking.API.Models.Snapshots
{
    public class AccountSnapshot
    {
        public Guid AggregateId { get; set; }
        public int Version { get; set; }
        public string Owner { get; set; } = string.Empty;
        public decimal Balance { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
