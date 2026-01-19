namespace eBoardAPI.Models.FundIncome
{
    public class FundIncomeDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public int ExpectedAmount { get; set; }
        public int CollectedAmount { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public string Description { get; set; } = string.Empty;
        public string Status  => CollectedAmount >= ExpectedAmount ? "Hoàn thành" : "Đang thu";
    }
}
