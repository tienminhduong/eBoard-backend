namespace eBoardAPI.Entities;

public class Class
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    
    public Guid GradeId { get; set; }
    public Grade Grade { get; set; } = null!;
    
    public Guid TeacherId { get; set; }
    public Teacher Teacher { get; set; } = null!;
    
    public string RoomName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    
    public int CurrentStudentCount { get; set; }
    public int MaxCapacity { get; set; }
}