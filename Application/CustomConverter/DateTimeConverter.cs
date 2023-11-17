using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Application.CustomConverter
{
    public class DateTimeConverter : JsonConverter<DateTime>
    {
        private string formatDate = "dd/MM/yyyy HH:mm:ss";
        private string timeZoneId = "SE Asia Standard Time";
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string dateString = reader.GetString();
            DateTime localDateTime = DateTime.ParseExact(dateString, formatDate, CultureInfo.InvariantCulture);

            TimeZoneInfo vietnamTimeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
            DateTime utcDateTime = TimeZoneInfo.ConvertTimeToUtc(localDateTime, vietnamTimeZone);

            return utcDateTime;
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            TimeZoneInfo vietnamTimeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
            DateTime localDateTime = TimeZoneInfo.ConvertTimeFromUtc(value, vietnamTimeZone);

            writer.WriteStringValue(localDateTime.ToString(formatDate));
        }
    }
}

