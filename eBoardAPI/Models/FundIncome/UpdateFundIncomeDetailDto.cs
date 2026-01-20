namespace eBoardAPI.Models.FundIncome
{
    public class UpdateFundIncomeDetailDto
    {
        public int? ContributedAmount { get; set; }
        public string? ContributedInfo { get; set; }
        public DateOnly? ContributedAt { get; set; }
        public string? Notes { get; set; }
    }
}
