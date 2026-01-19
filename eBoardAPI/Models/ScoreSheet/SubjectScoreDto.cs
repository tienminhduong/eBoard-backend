namespace eBoardAPI.Models.ScoreSheet;

public class SubjectScoreDto
{
    public Guid SubjectId { get; set; }
    public string SubjectName { get; set; } = string.Empty;
    public float MidtermScore { get; set; }
    public float FinalScore { get; set; }
    public float AverageScore { get; set; }
}