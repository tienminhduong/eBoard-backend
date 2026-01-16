namespace eBoardAPI.Models.Schedule;

public class UpdateScheduleSettingDetailDto
{
    public int PeriodNumber { get; set; }
    public bool IsMorningPeriod { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
}