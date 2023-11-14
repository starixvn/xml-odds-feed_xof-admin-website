namespace Fanex.OddsFeed.Admin.Utility
{
    using Microsoft.AspNetCore.Mvc;
    using System.Text.Json;
    using System.Text.Json.Serialization;

    public class CustomJsonResult : JsonResult
    {
        public CustomJsonResult(object value) : base(value)
        {
            SerializerSettings = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                Converters = { new CustomDateTimeConverter() }
            };
        }
    }

    public class CustomDateTimeConverter : JsonConverter<DateTime>
    {
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (DateTime.TryParse(reader.GetString(), out DateTime date))
            {
                return date;
            }
            return DateTime.MinValue;
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString("dd MMM yyyy HH:mm:ss"));
        }
    }
}
