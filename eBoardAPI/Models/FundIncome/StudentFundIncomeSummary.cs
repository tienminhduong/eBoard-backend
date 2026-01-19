namespace eBoardAPI.Models.FundIncome
{
    public class StudentFundIncomeSummary
    {
        public Guid StudentId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public int ExpectedAmount { get; set; }
        public int TotalContributedAmount { get; set; }
        public DateOnly? LatestContributedAt { get; set; }
        public string LatestNotes { get; set; } = string.Empty;
    }
}
