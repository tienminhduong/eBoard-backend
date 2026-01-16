using eBoardAPI.Models.Subject;

namespace eBoardAPI.Models.Schedule;

public class CreateClassPeriodDto
{
    public CreateSubjectDto Subject { get; set; } = null!;
    public int PeriodNumber { get; set; }
    public DayOfWeek DayOfWeek { get; set; }
    public required string TeacherName { get; set; }
    public string Notes { get; set; } = string.Empty;
    public Guid ClassId { get; set; }
    public bool IsMorningPeriod { get; set; }
}