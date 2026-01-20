namespace eBoardAPI.Models.Attendance;

public class AttendanceInfoByClassDto
{
    public Guid ClassId { get; set; }
    public required string ClassName { get; set; }
    public DateOnly Date { get; set; }
    public List<AttendanceDto> Attendances { get; set; } = [];
}