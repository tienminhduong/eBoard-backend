using Microsoft.EntityFrameworkCore;

namespace eBoardAPI.Entities;

[PrimaryKey(nameof(PeriodNumber), nameof(ScheduleSettingId))]
public class ScheduleSettingDetail
{
    public int PeriodNumber { get; set; }
    
    public Guid ScheduleSettingId { get; set; }
    public ScheduleSetting ScheduleSetting { get; set; } = null!;
    
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
}