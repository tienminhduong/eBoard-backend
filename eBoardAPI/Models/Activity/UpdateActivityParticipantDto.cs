namespace eBoardAPI.Models.Activity;

public class UpdateActivityParticipantDto
{
    public string ParentPhoneNumber { get; set; } = string.Empty;
    public string TeacherComments { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
}