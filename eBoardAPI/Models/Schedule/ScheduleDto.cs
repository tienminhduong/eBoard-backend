using eBoardAPI.Models.Class;

namespace eBoardAPI.Models.Schedule;

public class ScheduleDto
{
    public ClassInfoDto Class { get; set; } = null!;
    public IEnumerable<ClassPeriodDto> ClassPeriods { get; set; } = [];
}