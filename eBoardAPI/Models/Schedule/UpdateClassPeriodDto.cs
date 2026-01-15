using eBoardAPI.Models.Subject;

namespace eBoardAPI.Models.Schedule;

public class UpdateClassPeriodDto
{
    public string TeacherName { get; set; } = string.Empty;
    public CreateSubjectDto Subject { get; set; } = null!;
    public string Notes { get; set; } = string.Empty;
}