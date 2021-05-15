using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Zindagi.Domain;
using Zindagi.SeedWork;

namespace Zindagi.Infra.JsonConverters
{
    public class StatusConverter : JsonConverter<Status>
    {
        public override Status Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var value = reader.GetInt32();
            if (value == default)
                value = 0;

            return Enumeration.FromValue<Status>(value);
        }

        public override void Write(Utf8JsonWriter writer, Status value, JsonSerializerOptions options)
            => writer.WriteNumberValue(value.Id);
    }
}
