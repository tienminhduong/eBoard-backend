namespace eBoardAPI.Models.FundIncome
{
    public class CreateFundIncomeDetailDto
    {
        public Guid FundIncomeId { get; set; }
        public Guid StudentId { get; set; }
        public int ContributedAmount { get; set; }
        public string ContributedInfo { get; set; } = string.Empty;
        public string ContributionStatus { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
    }
}
