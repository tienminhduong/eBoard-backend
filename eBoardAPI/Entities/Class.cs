using System.ComponentModel.DataAnnotations;

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
    public string Description { get; set; } = string.Empty;
    
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    
    public int CurrentStudentCount { get; set; }
    public int MaxCapacity { get; set; }
}