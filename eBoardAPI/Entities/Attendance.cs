using eBoardAPI.Consts;

namespace eBoardAPI.Entities;

public class Attendance
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public Guid StudentId { get; set; }
    public Guid ClassId { get; set; }
    public DateOnly Date { get; set; }
    public string Status { get; set; } = EAttendanceStatus.PRESENT;
    public string AbsenceReason { get; set; } = string.Empty;
    public string PickupPerson { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
}