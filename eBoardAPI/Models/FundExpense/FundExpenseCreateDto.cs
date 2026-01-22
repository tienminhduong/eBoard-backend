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
        public IFormFile? Image { get; set; }

        public string ValidateData()
        {
            if (string.IsNullOrWhiteSpace(Title))
            {
                return "Title is required.";
            }
            if (Amount <= 0)
            {
                return "Amount must be greater than zero.";
            }
            if (string.IsNullOrWhiteSpace(SpenderName))
            {
                return "Spender name is required.";
            }
            //if (ExpenseDate > DateOnly.FromDateTime(DateTime.Now))
            //{
            //    return "Expense date cannot be in the future.";
            //}
            return string.Empty;
        }
    }
}