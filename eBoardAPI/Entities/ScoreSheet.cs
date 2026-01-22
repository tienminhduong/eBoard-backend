namespace eBoardAPI.Entities;

public class ScoreSheet
{
    public Guid Id { get; init; } = Guid.NewGuid();
    
    public Guid StudentId { get; set; }
    public Student Student { get; set; } = null!;
    
    public Guid ClassId { get; set; }
    public Class Class { get; set; } = null!;
    
    public float AverageScore { get; set; }
    public string Grade { get; set; } = string.Empty;
    public int Semester { get; set; }
    public string Conduct { get; set; } = string.Empty;
    public int Rank { get; set; }
    
    public ICollection<ScoreSheetDetail> Details { get; set; } = [];
}