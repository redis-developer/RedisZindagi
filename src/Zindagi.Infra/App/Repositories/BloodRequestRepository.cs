using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NReJSON;
using StackExchange.Redis;
using Zindagi.Domain;
using Zindagi.Domain.RequestsAggregate;
using Zindagi.Infra.Redis;
using Zindagi.SeedWork;

namespace Zindagi.Infra.App.Repositories
{
    public class BloodRequestRepository : IBloodRequestRepository
    {
        private readonly IConnectionMultiplexer _connectionMultiplexer;
        private readonly JsonSerializerOptions _jsonSerializerOptions;
        private readonly ILogger<UserRepository> _logger;

        public BloodRequestRepository(IConnectionMultiplexer connectionMultiplexer, JsonSerializerOptions jsonSerializerOptions, ILogger<UserRepository> logger)
        {
            _connectionMultiplexer = connectionMultiplexer;
            _jsonSerializerOptions = jsonSerializerOptions;
            _logger = logger;
        }

        public async Task<BloodRequest> CreateAsync(BloodRequest request)
        {
            var db = _connectionMultiplexer.GetDatabase();
            var json = JsonSerializer.Serialize(request, _jsonSerializerOptions);
            var persistenceResult = await db.JsonSetAsync(request.GetPersistenceKey(), json);

            _logger.LogDebug("[User] [INSERT] {json} [{result}]", json, persistenceResult.IsSuccess);

            await db.PublishAsync(RedisConstants.NewBloodRequestChannel, json);
            return request;
        }

        public async Task<BloodRequest> GetAsync(Guid id)
        {
            var db = _connectionMultiplexer.GetDatabase();
            var result = await db.JsonGetAsync<BloodRequest>(RedisConstants.GetBloodRequestPersistenceKey(id));
            return result;
        }

        public async Task<bool> UpdateRequestStatus(Guid id, OpenIdKey openIdKey, DetailedStatusList status)
        {
            var db = _connectionMultiplexer.GetDatabase();
            await db.JsonSetAsync(RedisConstants.GetBloodRequestPersistenceKey(id), JsonSerializer.Serialize(status, _jsonSerializerOptions), ".Status");

            var path = status switch
            {
                DetailedStatusList.Assigned => ".AssignedBy",
                DetailedStatusList.Cancelled => ".CancelledBy",
                _ => ".ActionBy",
            };
            var result = await db.JsonSetAsync(RedisConstants.GetBloodRequestPersistenceKey(id), JsonSerializer.Serialize(openIdKey.GetPersistenceKey(), _jsonSerializerOptions), path);
            return result.IsSuccess;
        }
    }
}
