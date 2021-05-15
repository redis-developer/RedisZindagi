using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Zindagi.SeedWork;

namespace Zindagi.Infra.JsonConverters
{
    public class OpenIdKeyConverter : JsonConverter<OpenIdKey>
    {
        public override OpenIdKey Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var value = reader.GetString();
            if (string.IsNullOrWhiteSpace(value))
                return null!;

            return OpenIdKey.Create(value);
        }

        public override void Write(Utf8JsonWriter writer, OpenIdKey value, JsonSerializerOptions options)
            => writer.WriteStringValue(value.Value);
    }
}
