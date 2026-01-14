namespace eBoardAPI.Entities;

public class FundExpense
{
    public Guid Id { get; set; }
    
    public Guid ClassFundId { get; set; }
    public ClassFund ClassFund { get; set; } = null!;
    
    public string Title { get; set; } = string.Empty;
    public int Amount { get; set; }
    public DateOnly ExpenseDate { get; set; }
    public string SpenderName { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
    public string InvoiceImgUrl { get; set; } = string.Empty;
}