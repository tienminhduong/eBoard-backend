namespace eBoardAPI.Models.ScoreSheet;

public class UpdateSubjectScoreDto
{
    public Guid SubjectId { get; set; }
    public float MidtermScore { get; set; }
    public float FinalScore { get; set; }
}