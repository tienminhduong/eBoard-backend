namespace eBoardAPI.Models.Schedule;

public class ScheduleSettingDetailDto
{
    public int PeriodNumber { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public bool IsMorningPeriod { get; set; }
}