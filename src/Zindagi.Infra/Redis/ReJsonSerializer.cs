using System.Text.Json;
using NReJSON;
using StackExchange.Redis;

namespace Zindagi.Infra.Redis
{
    public class ReJsonSerializerProxy : ISerializerProxy
    {
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public ReJsonSerializerProxy(JsonSerializerOptions jsonSerializerOptions) => _jsonSerializerOptions = jsonSerializerOptions;

        public TResult Deserialize<TResult>(RedisResult serializedValue)
        {
            try
            {
                var value = serializedValue?.ToString() ?? string.Empty;
                return JsonSerializer.Deserialize<TResult>(value, _jsonSerializerOptions)!;
            }
            catch
            {
                return default!;
            }
        }

        public string Serialize<TObjectType>(TObjectType obj) => JsonSerializer.Serialize(obj);
    }
}
