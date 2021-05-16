using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NReJSON;
using StackExchange.Redis;
using Zindagi.Domain.UserAggregate;
using Zindagi.Domain.UserAggregate.Commands;
using Zindagi.SeedWork;

namespace Zindagi.Infra.App.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IConnectionMultiplexer _connectionMultiplexer;
        private readonly JsonSerializerOptions _jsonSerializerOptions;
        private readonly ILogger<UserRepository> _logger;

        public UserRepository(IConnectionMultiplexer connectionMultiplexer, JsonSerializerOptions jsonSerializerOptions, ILogger<UserRepository> logger)
        {
            _connectionMultiplexer = connectionMultiplexer;
            _jsonSerializerOptions = jsonSerializerOptions;
            _logger = logger;
        }

        public async Task<User?> CreateAsync(User newUser)
        {
            var db = _connectionMultiplexer.GetDatabase();
            var json = JsonSerializer.Serialize(newUser, _jsonSerializerOptions);
            var persistenceResult = await db.JsonSetAsync(newUser.GetPersistenceKey(), json);

            _logger.LogDebug("[User] [INSERT] {json} [{result}]", json, persistenceResult.IsSuccess);
            return await GetAsync(newUser.AlternateId);
        }

        public async Task<User> GetAsync(OpenIdKey openIdKey)
        {
            var db = _connectionMultiplexer.GetDatabase();
            var user = await db.JsonGetAsync<User>(openIdKey.GetPersistenceKey());
            if (string.IsNullOrWhiteSpace(user.FullName))
                user.FirstName = user.Email;
            return user;
        }

        public async Task<User> UpdateAsync(OpenIdKey openIdKey, UpdateUserInfo userInfo)
        {
            var db = _connectionMultiplexer.GetDatabase();
            var user = await db.JsonGetAsync<User>(openIdKey.GetPersistenceKey());
            user.Update(userInfo);

            var json = JsonSerializer.Serialize(user, _jsonSerializerOptions);
            var persistenceResult = await db.JsonSetAsync(user.GetPersistenceKey(), json);

            _logger.LogDebug("[User] [UPDATE] {json} [{result}]", json, persistenceResult.IsSuccess);

            return user;
        }

        public async Task<int> DeleteAsync(OpenIdKey openIdKey)
        {
            var db = _connectionMultiplexer.GetDatabase();
            return await db.JsonDeleteAsync(openIdKey.GetPersistenceKey());
        }
    }
}
