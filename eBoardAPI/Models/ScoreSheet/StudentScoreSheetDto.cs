namespace eBoardAPI.Models.ScoreSheet;

public class StudentScoreSheetDto
{
    public Guid StudentId { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public string ClassName { get; set; } = string.Empty;
    public string AcademicYear { get; set; } = string.Empty;
    public int Semester { get; set; }
    public int Rank { get; set; }
    public string RankInClass { get; set; } = string.Empty;
    public double AverageScore { get; set; }
    public string Grade { get; set; } = string.Empty;
    public string Conduct { get; set; } = string.Empty;
    public string FinalGrade { get; set; } = string.Empty;
    public IEnumerable<SubjectScoreDto> SubjectScores { get; set; } = [];
}