namespace eBoardAPI.Models.Activity;

public class UpdateActivityDto
{
    public required string Name { get; set; }
    public required string Location { get; set; }
    public int MaxParticipants { get; set; }
    public string InChargeTeacher { get; set; } = string.Empty;
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public int Cost { get; set; }
    public DateTime AssignDeadline { get; set; }
    public string Description { get; set; } = string.Empty;
}