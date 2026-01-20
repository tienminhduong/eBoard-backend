namespace eBoardAPI.Models.Attendance;

public class CreateAttendaceForDateDto
{
    public required Guid ClassId { get; set; }
    public required DateOnly Date { get; set; }
}