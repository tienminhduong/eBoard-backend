namespace eBoardAPI.Entities;

public class ClassFund
{
    public Guid Id { get; set; }
    
    public Guid ClassId { get; set; }
    public Class Class { get; set; } = null!;
    
    public int CurrentBalance { get; set; }
    public int TotalContributions { get; set; }
    public int TotalExpenses { get; set; }
}