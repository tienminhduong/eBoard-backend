namespace eBoardAPI.Models.Class;

public class ClassInfoDto
{
    public required string Name { get; set; }
    public required string Grade { get; set; }
    public required string TeacherName { get; set; }
    public string RoomName { get; set; } = string.Empty;
    public string AcademicYear { get; set; } = string.Empty;
    public int MaxCapacity { get; set; }
    public string Description { get; set; } = string.Empty;
}