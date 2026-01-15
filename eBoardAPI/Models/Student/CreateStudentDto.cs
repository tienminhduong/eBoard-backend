namespace eBoardAPI.Models.Student;

public class CreateStudentDto
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public DateOnly DateOfBirth { get; set; }
    public string Address { get; set; }
    public string Province { get; set; } = string.Empty;
    public string District { get; set; } = string.Empty;
    public string Ward { get; set; } = string.Empty;
    public string ParentPhoneNumber { get; set; }
    public string RelationshipWithParent { get; set; }
    public string ParentFullName { get; set; }
    public string ParentHealthCondition { get; set; }
    public Guid ClassId { get; set; }
}