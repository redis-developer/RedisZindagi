using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Zindagi.Domain;
using Zindagi.SeedWork;

namespace Zindagi.Infra.JsonConverters
{
    public class BloodGroupConverter : JsonConverter<BloodGroup>
    {
        public override BloodGroup? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var value = reader.GetInt32();
            if (value == default)
                value = 0;

            return Enumeration.FromValue<BloodGroup>(value);
        }

        public override void Write(Utf8JsonWriter writer, BloodGroup value, JsonSerializerOptions options)
            => writer.WriteNumberValue(value.Id);
    }
}
