namespace eBoardAPI.Entities;

public class ScheduleSetting
{
    public Guid Id { get; init; } = Guid.NewGuid();
    
    public Guid ClassId { get; set; }
    public Class Class { get; set; } = null!;
    
    public int MorningPeriodCount { get; set; }
    public int AfternoonPeriodCount { get; set; }
}