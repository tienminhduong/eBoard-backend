namespace eBoardAPI.Models.ScoreSheet;

public class StudentScoreBySubjectDto
{
    public Guid StudentId { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public float MidtermScore { get; set; }
    public float FinalScore { get; set; }
    public float AverageScore { get; set; }
    public string Grade { get; set; } = string.Empty;
}