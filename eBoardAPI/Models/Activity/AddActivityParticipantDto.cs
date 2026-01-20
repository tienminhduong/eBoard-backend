namespace eBoardAPI.Models.Activity;

public class AddActivityParticipantDto
{
    public Guid StudentId { get; set; }
    public Guid ActivityId { get; set; }
    public string ParentPhoneNumber { get; set; } = string.Empty;
    public string TeacherComments { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
}