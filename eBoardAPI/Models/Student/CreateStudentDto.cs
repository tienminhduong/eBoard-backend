using eBoardAPI.Consts;

namespace eBoardAPI.Models.Student;

public class CreateStudentDto
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public DateOnly DateOfBirth { get; set; }
    public string Address { get; set; } = string.Empty;
    public string Province { get; set; } = string.Empty;
    public string District { get; set; } = string.Empty;
    public string Ward { get; set; } = string.Empty;
    public string Gender { get; set; } = EGender.MALE;
    public string ParentPhoneNumber { get; set; } = string.Empty;
    public string RelationshipWithParent { get; set; } = string.Empty;
    public string ParentFullName { get; set; } = string.Empty;
    public string ParentHealthCondition { get; set; } = string.Empty;
    public Guid ClassId { get; set; }
}