namespace eBoardAPI.Models.Class;

public class CreateClassDto
{
    public Guid TeacherId { get; set; }
    public required string ClassName { get; set; }
    public required Guid GradeId { get; set; }
    public int AcademicStartYear { get; set; }
    public int AcademicEndYear { get; set; }
    public int MaxCapacity { get; set; }
    public required string RoomName { get; set; }
    public string Description { get; set; } = string.Empty;
}