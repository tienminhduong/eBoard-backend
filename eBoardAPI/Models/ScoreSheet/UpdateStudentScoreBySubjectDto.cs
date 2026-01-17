namespace eBoardAPI.Models.ScoreSheet;

public class UpdateStudentScoreBySubjectDto
{
    public Guid StudentId { get; set; }
    public float MidtermScore { get; set; }
    public float FinalScore { get; set; }
}