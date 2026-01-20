namespace eBoardAPI.Models.Attendance;

public class PatchAttendanceDto
{
    public string? Status { get; set; }
    public string? AbsenceReason { get; set; }
    public string? PickupPerson { get; set; }
    public string? Notes { get; set; }
}