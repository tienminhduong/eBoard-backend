namespace eBoardAPI.Models.Attendance;

public class ClassAttendanceSummary
{
    public int StudentCount { get; set; }
    public int CurrentAttendance { get; set; }
    public int AbsentWithExcuse { get; set; }
    public int AbsentWithoutExcuse { get; set; }
}