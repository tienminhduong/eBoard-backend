namespace eBoardAPI.Models.FundIncome
{
    public class FundIncomeDetailDto
    {
        public Guid Id { get; set; }
        public int ContributedAmount { get; set; }
        public string ContributedInfo { get; set; } = string.Empty;
        public DateOnly ContributedAt { get; set; }
        public string ContributionStatus { get; set; } = string.Empty;
        public DateOnly Deadline { get; set; }
        public string Notes { get; set; } = string.Empty;
    }
}
