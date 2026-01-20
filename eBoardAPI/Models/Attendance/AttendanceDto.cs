namespace eBoardAPI.Models.Attendance;

public class AttendanceDto
{
    public Guid Id { get; set; }
    public Guid StudentId { get; set; }
    public required string StudentName { get; set; }
    public required string Status { get; set; }
    public required string AbsenceReason { get; set; }
    public required string PickupPerson { get; set; }
    public required string Notes { get; set; }
}