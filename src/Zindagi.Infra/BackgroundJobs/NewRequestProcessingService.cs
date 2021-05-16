using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using Zindagi.Domain.RequestsAggregate;
using Zindagi.Domain.RequestsAggregate.Notifications;
using Zindagi.Infra.Redis;

namespace Zindagi.Infra.BackgroundJobs
{
    public class NewRequestProcessingService : BackgroundService
    {
        private const string SERVICE_NAME = nameof(NewRequestProcessingService);
        private readonly ILogger<NewRequestProcessingService> _logger;
        private readonly IConnectionMultiplexer _multiplexer;
        private readonly JsonSerializerOptions _jsonSerializerOptions;
        private readonly IMediator _mediator;

        public NewRequestProcessingService(ILogger<NewRequestProcessingService> logger, JsonSerializerOptions jsonSerializerOptions, IConnectionMultiplexer multiplexer, IMediator mediator)
        {
            _logger = logger;
            _jsonSerializerOptions = jsonSerializerOptions;
            _multiplexer = multiplexer;
            _mediator = mediator;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"{SERVICE_NAME} is starting.");

            stoppingToken.Register(() =>
                _logger.LogInformation($"{SERVICE_NAME} background task is stopping."));

            await _multiplexer.GetSubscriber().SubscribeAsync(RedisConstants.NewBloodRequestChannel, async (channel, message) =>
            {
                if (message.IsNullOrEmpty)
                    return;

                var request = JsonSerializer.Deserialize<BloodRequest>(message.ToString(), _jsonSerializerOptions);
                if (request == null)
                    return;

                _logger.LogDebug("[INSERT] [BloodRequest] [Redis] [PubSub] {channel} {message}", channel, message);

                await _mediator.Publish(new BloodRequestCreated(request), stoppingToken);

                await Task.CompletedTask;
            });
        }
    }
}
