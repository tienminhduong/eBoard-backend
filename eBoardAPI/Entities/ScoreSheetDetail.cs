using Microsoft.EntityFrameworkCore;

namespace eBoardAPI.Entities;

[PrimaryKey(nameof(ScoreSheetId), nameof(SubjectId))]
public class ScoreSheetDetail
{
    public Guid ScoreSheetId { get; set; }
    public ScoreSheet ScoreSheet { get; set; } = null!;
    
    public Guid SubjectId { get; set; }
    public Subject Subject { get; set; } = null!;
    
    public float MidtermScore { get; set; }
    public float FinalScore { get; set; }
    public float AverageScore { get; set; }
}