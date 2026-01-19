namespace eBoardAPI.Models.ScoreSheet;

public class StudentScoreSummaryDto
{
    public Guid StudentId { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public double AverageScore { get; set; }
    public string Grade { get; set; } = string.Empty;
}