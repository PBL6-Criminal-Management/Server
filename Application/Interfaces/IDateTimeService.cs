namespace Application.Interfaces
{
    public interface IDateTimeService
    {
        DateTime NowUtc { get; }

        bool IsCorrectFormat(string datetime, string format, out DateTime result);
        bool IsBetween(string strDates, string datetime);
        string ConvertToUtc(DateTime TimeLocal);
    }
}