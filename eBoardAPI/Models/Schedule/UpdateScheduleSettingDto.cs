namespace eBoardAPI.Models.Schedule;

public class UpdateScheduleSettingDto
{
    public int MorningPeriodCount { get; set; }
    public int AfternoonPeriodCount { get; set; }
    public IEnumerable<UpdateScheduleSettingDetailDto> Details { get; set; } = [];
}