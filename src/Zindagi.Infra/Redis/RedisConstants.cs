using System;
using StackExchange.Redis;
using Zindagi.Domain.RequestsAggregate;

namespace Zindagi.Infra.Redis
{
    public static class RedisConstants
    {
        private static readonly string BloodRequestName = nameof(BloodRequest).ToUpperInvariant();

        public static readonly RedisChannel NewBloodRequestChannel = new($"URN:{BloodRequestName}:NEW", RedisChannel.PatternMode.Auto);
        public static readonly string BloodRequestsSearchSchema = "blood_requests";

        public static string GetBloodRequestPersistenceKey(Guid id) => $"{BloodRequestName}:{id}".ToUpperInvariant();
        public static string GetBloodRequestPersistenceKey(string id) => $"{BloodRequestName}:{id}".ToUpperInvariant();
    }
}
