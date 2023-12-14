namespace Application.Features.Statistic.Queries.CriminalStructure
{
    public class CriminalSituationDevelopmentsResponse
    {
        public int Month { get; set; }
        public long CaseCount { get; set; }
        public long CriminalCount { get; set; }
        public long VictimCount { get; set; }
    }
}
