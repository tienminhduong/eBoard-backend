namespace eBoardAPI.Entities;

public class Class
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    
    public Guid? GradeId { get; set; }
    public Grade? Grade { get; set; }
    
    public Guid? TeacherId { get; set; }
    public Teacher? Teacher { get; set; }
    
    public string RoomName { get; set; } = string.Empty;
    public string AcademicYear { get; set; } = string.Empty;
    public int MaxCapacity { get; set; }
    public string Description { get; set; } = string.Empty;
}