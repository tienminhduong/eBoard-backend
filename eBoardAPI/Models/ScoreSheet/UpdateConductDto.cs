namespace eBoardAPI.Models.ScoreSheet;

public class UpdateConductDto
{
    public Guid StudentId { get; set; }
    public string Conduct { get; set; } = string.Empty;
}