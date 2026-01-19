namespace eBoardAPI.Entities;

public enum PaymentStatus
{
    FULL,
    PARTIAL
}
public class FundIncomeDetail
{
    public Guid Id { get; set; }
    
    public Guid FundIncomeId { get; set; }
    public FundIncome FundIncome { get; set; } = null!;
    
    public Guid StudentId { get; set; }
    public Student Student { get; set; } = null!;
    
    public int ContributedAmount { get; set; }
    public string ContributedInfo { get; set; } = string.Empty;
    public DateOnly ContributedAt { get; set; }
    public string ContributionStatus { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;

    public void UpdateStatus(int expectAmount)
    {
        ContributionStatus = ContributedAmount >= expectAmount ? PaymentStatus.FULL.ToString() : PaymentStatus.PARTIAL.ToString();
    }
}