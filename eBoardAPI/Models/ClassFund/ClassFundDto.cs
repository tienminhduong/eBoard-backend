using eBoardAPI.Models.Class;

namespace eBoardAPI.Models.ClassFund;

public class ClassFundDto
{
    public Guid Id { get; set; }
    public string ClassName { get; set; } = string.Empty;
    public string AcademicYear { get; set; } = string.Empty;
    public int CurrentBalance { get; set; }
    public int TotalContributions { get; set; }
    public int TotalExpenses { get; set; }
}