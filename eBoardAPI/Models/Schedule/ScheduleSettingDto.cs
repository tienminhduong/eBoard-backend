namespace eBoardAPI.Models.Schedule;

public class ScheduleSettingDto
{
    public Guid Id { get; set; }
    public int MorningPeriodCount { get; set; }
    public int AfternoonPeriodCount { get; set; }
    public IEnumerable<ScheduleSettingDetailDto> Details { get; set; } = [];
}