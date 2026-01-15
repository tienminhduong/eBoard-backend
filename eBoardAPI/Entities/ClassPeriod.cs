using eBoardAPI.Consts;

namespace eBoardAPI.Entities;

public class ClassPeriod
{
    public Guid Id { get; init; } = Guid.NewGuid();
    
    public Guid ClassId { get; set; }
    public Class Class { get; set; } = null!;
    
    public Guid SubjectId { get; set; }
    public Subject Subject { get; set; } = null!;

    public DayOfWeek DayOfWeek { get; set; } = DayOfWeek.Monday;
    public int PeriodNumber { get; set; }
    public string TeacherName { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
}