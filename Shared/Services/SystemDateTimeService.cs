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
            DateTime Date = DateTime.ParseExact(datetime, "dd/MM/yyyy HH:mm:ss", cultureInfo);
            if (dates.Length == 2)
            {
                startDate = DateTime.ParseExact(dates[0], "dd/MM/yyyy HH:mm:ss", cultureInfo);
                endDate = DateTime.ParseExact(dates[1], "dd/MM/yyyy HH:mm:ss", cultureInfo);
            }
            else
            {
                startDate = DateTime.ParseExact(dates[0], "dd/MM/yyyy HH:mm:ss", cultureInfo);
                endDate = DateTime.MaxValue;
            }

            return Date >= startDate && Date <= endDate;
        }
        public string ConvertToUtc(DateTime timeLocal)
        {
            DateTimeOffset dateTimeOffset = timeLocal.ToUniversalTime();
            string utcTime = dateTimeOffset.ToString("dd/MM/yyyy HH:mm:ss");
            return utcTime;
        }
    }
}