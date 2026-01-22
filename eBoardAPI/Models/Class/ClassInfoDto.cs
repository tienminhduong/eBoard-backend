namespace eBoardAPI.Models.Class;

public class ClassInfoDto
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Grade { get; set; }
    public required string TeacherName { get; set; }
    public Guid TeacherId { get; set; }
    public string RoomName { get; set; } = string.Empty;
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public int CurrentStudentCount { get; set; }
    public int MaxCapacity { get; set; }
    public string Description { get; set; } = string.Empty;
}