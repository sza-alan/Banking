namespace Banking.API.IntegrationEvents
{
    public interface IIntegrationEventPublisher
    {
        Task PublishAsync(IIntegrationEvent integrationEvent, CancellationToken cancellationToken);
    }
}
