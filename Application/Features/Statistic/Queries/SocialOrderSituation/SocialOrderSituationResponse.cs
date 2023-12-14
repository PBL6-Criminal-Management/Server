namespace Application.Features.Statistic.Queries.CriminalStructure
{
    public class SocialOrderSituationResponse
    {
        public int Month { get; set; }
        public int Year { get; set; }
        public long CaseCount { get; set; }
        public long TriedCaseCount { get; set; }
        public long ArrestedOrHandledCriminalCount { get; set; }
    }
}
