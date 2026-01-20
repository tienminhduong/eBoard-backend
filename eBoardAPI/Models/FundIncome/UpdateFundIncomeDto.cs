namespace eBoardAPI.Models.FundIncome
{
    public class UpdateFundIncomeDto
    {
        public string? Title { get; set; }
        public int? AmountPerStudent { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public string? Description { get; set; }
    }
}
