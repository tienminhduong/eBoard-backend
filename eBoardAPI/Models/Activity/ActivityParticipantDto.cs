namespace eBoardAPI.Models.Activity;

public class ActivityParticipantDto
{
    public Guid Id { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public string ParentPhoneNumber { get; set; } = string.Empty;
    public string TeacherComments { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
}