namespace eBoardAPI.Models.Violation
{
    public class ViolationsStatsDto
    {
        public int TotalViolations { get; set; }
        public int UnreadViolations { get; set; }
        public int ServereViolations { get; set; }
        public int ThisWeekViolations { get; set; }
    }
}
