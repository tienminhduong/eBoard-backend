namespace eBoardAPI.Models.Activity;

public class ExtracurricularActivityDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public int MaxParticipants { get; set; }
    public string InChargeTeacher { get; set; } = string.Empty;
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public int Cost { get; set; }
    public DateTime AssignDeadline { get; set; }
    public string Description { get; set; } = string.Empty;
    public IEnumerable<ActivityParticipantDto> Participants { get; set; } = [];
}