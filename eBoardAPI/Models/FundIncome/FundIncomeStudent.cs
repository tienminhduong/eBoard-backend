namespace eBoardAPI.Models.FundIncome
{
    public class FundIncomeStudent
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public int ExpectedAmount { get; set; }
        public int PaidAmount { get; set; }
        public DateOnly EndDate { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}
