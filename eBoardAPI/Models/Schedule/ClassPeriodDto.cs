using eBoardAPI.Entities;
using eBoardAPI.Models.Subject;

namespace eBoardAPI.Models.Schedule;

public class ClassPeriodDto
{
    public Guid Id { get; set; }
    public DayOfWeek DayOfWeek { get; set; }
    public int PeriodNumber { get; set; }
    public string TeacherName { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
    public SubjectDto Subject { get; set; } = null!;
}