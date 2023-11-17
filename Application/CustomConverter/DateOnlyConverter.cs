using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Application.CustomConverter
{
    public class DateOnlyConverter : JsonConverter<DateOnly>
    {
        private string formatDate = "dd/MM/yyyy";
        private string timeZoneId = "SE Asia Standard Time";
        public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string dateString = reader.GetString();
            DateTime localDateTime = DateTime.ParseExact(dateString, formatDate, CultureInfo.InvariantCulture);

            TimeZoneInfo vietnamTimeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
            DateOnly utcDateTime = DateOnly.FromDateTime(TimeZoneInfo.ConvertTimeToUtc(localDateTime, vietnamTimeZone));

            return utcDateTime;
        }

        public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
        {
            TimeZoneInfo vietnamTimeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
            DateOnly localDateTime = DateOnly.FromDateTime(TimeZoneInfo.ConvertTimeFromUtc(value.ToDateTime(new TimeOnly()), vietnamTimeZone));

            writer.WriteStringValue(localDateTime.ToString(formatDate));
        }
    }
}

