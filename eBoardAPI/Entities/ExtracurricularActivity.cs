namespace eBoardAPI.Entities;

public class ExtracurricularActivity
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public Guid ClassId { get; set; }
    public Class Class { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Location { get; set; } = null!;
    public int MaxParticipants { get; set; }
    public string InChargeTeacher { get; set; } = string.Empty;
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public int Cost { get; set; }
    public DateTime AssignDeadline { get; set; }
    public string Description { get; set; } = string.Empty;
    
}