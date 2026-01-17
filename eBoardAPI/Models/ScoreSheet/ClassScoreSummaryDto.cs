namespace eBoardAPI.Models.ScoreSheet;

public class ClassScoreSummaryDto
{
    public int ExcellentCount { get; set; }
    public int GoodCount { get; set; }
    public int AverageCount { get; set; }
    public int PoorCount { get; set; }

    public IEnumerable<StudentScoreSummaryDto> StudentScores { get; set; } = [];
}