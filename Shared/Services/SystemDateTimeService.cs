using Application.Interfaces;
using System.Data.Common;
using System.Globalization;

namespace Shared.Services
{
    public class SystemDateTimeService : IDateTimeService
    {
        public DateTime NowUtc => DateTime.UtcNow;

        public bool IsCorrectFormat(string datetime, string format, out DateTime result)
        {
            if (DateTime.TryParseExact(datetime, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out result))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool IsBetween(string strDates, string datetime)
        {
            string[] dates = strDates.Split('-');
            CultureInfo cultureInfo = new CultureInfo("vi-VN");
            DateTime startDate;
            DateTime endDate;
            DateTime Date = DateTime.ParseExact(datetime, "HH:mm dd/MM/yyyy", cultureInfo);
            if (dates.Length == 2)
            {
                startDate = DateTime.ParseExact(dates[0], "HH:mm dd/MM/yyyy", cultureInfo);
                endDate = DateTime.ParseExact(dates[1], "HH:mm dd/MM/yyyy", cultureInfo);
            }
            else
            {
                startDate = DateTime.ParseExact(dates[0], "HH:mm dd/MM/yyyy", cultureInfo);
                endDate = DateTime.MaxValue;
            }

            return Date >= startDate && Date <= endDate;
        }
        public string ConvertToUtc(DateTime timeLocal)
        {
            DateTimeOffset dateTimeOffset = timeLocal.ToUniversalTime();
            string utcTime = dateTimeOffset.ToString("HH:mm dd/MM/yyyy");
            return utcTime;
        }
    }
}