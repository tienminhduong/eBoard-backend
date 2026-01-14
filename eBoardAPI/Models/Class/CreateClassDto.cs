namespace eBoardAPI.Models.Class;

public class CreateClassDto
{
    public required string Name { get; set; }
    public required Guid GradeId { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public int MaxCapacity { get; set; }
    public required string RoomName { get; set; }
    public string Description { get; set; } = string.Empty;
}