namespace eBoardAPI.Entities;

public class FundIncome
{
    public Guid Id { get; set; }
    
    public Guid ClassFundId { get; set; }
    public ClassFund? ClassFund { get; set; }
    
    public string Title { get; set; } = string.Empty;
    public int ExpectedAmount { get; set; }
    public int CollectedAmount { get; set; }
    public int AmountPerStudent { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public string Description { get; set; } = string.Empty;

    public void CalculateExpectedAmount(int numberOfStudents)
    {
        ExpectedAmount = AmountPerStudent * numberOfStudents;
    }

}