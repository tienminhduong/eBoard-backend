using eBoardAPI.Models.Subject;

namespace eBoardAPI.Models.Schedule;

public class UpdateClassPeriodDto
{
    public string? TeacherName { get; set; }
    public CreateSubjectDto? Subject { get; set; }
    public string? Notes { get; set; }
    public int? PeriodNumber { get; set; }
    public DayOfWeek? DayOfWeek { get; set; }
}