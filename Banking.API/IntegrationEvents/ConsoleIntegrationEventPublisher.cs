
namespace Banking.API.IntegrationEvents
{
    public class ConsoleIntegrationEventPublisher : IIntegrationEventPublisher
    {
        private readonly ILogger _logger;

        public ConsoleIntegrationEventPublisher(ILogger logger)
        {
            _logger = logger;
        }

        public Task PublishAsync(IIntegrationEvent integrationEvent, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Integration Event publicado: {@IntegrationEvent}", integrationEvent);

            return Task.CompletedTask;
        }
    }
}
