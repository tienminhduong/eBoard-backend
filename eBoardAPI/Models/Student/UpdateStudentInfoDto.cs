namespace eBoardAPI.Models.Student;

public class UpdateStudentInfoDto
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? RelationshipWithParent { get; set; }
    public DateOnly? DateOfBirth { get; set; }
    public string? Gender { get; set; }
    public string? Address { get; set; }
    public string? Province { get; set; }
    public string? District { get; set; }
    public string? Ward { get; set; }
}