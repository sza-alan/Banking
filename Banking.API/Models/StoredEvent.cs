namespace Banking.API.Models
{
    public class StoredEvent
    {
        public Guid Id { get; set; }
        public Guid AggregateId { get; set; }
        public int Version { get; set; }
        public string EventType { get; set; } = string.Empty;
        public string Data { get; set; } = string.Empty;
        public DateTime OccurredAt { get; set; }
    }
}
