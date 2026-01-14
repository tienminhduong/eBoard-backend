namespace eBoardAPI.Models.Student;

public class CreateStudentDto
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public DateOnly DateOfBirth { get; set; }
    public string Address { get; set; }
    public Guid ProvinceId { get; set; }
    public Guid DistrictId { get; set; }
    public Guid WardId { get; set; }
    public string ParentPhoneNumber { get; set; }
    public string RelationshipWithParent { get; set; }
    public string ParentFullName { get; set; }
    public string ParentHealthCondition { get; set; }
    public Guid ClassId { get; set; }
}