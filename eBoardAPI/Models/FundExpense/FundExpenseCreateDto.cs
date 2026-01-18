namespace eBoardAPI.Models.FundExpense
{
    public class FundExpenseCreateDto
    {
        public string Title { get; set; } = string.Empty;
        public int Amount { get; set; }
        public string SpenderName { get; set; } = string.Empty;
        public DateOnly ExpenseDate { get; set; }
        public string InvoiceImgUrl { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
    }
}