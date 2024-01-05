using Application.Exceptions;
using Domain.Constants;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Application.CustomConverter
{
    public class DateOnlyConverter : JsonConverter<DateOnly>
    {
        private string formatDate = "dd/MM/yyyy";
        public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string? dateString = reader.GetString();
            if(DateTime.TryParseExact(dateString, formatDate, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime localDateTime))
            {
                return DateOnly.FromDateTime(localDateTime);
            }
            throw new ApiException(StaticVariable.NOT_CORRECT_DATE_FORMAT);
        }

        public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString(formatDate));
        }
    }
}

