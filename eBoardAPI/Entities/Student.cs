using eBoardAPI.Consts;
namespace eBoardAPI.Entities;


public class Student
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public Guid ParentId { get; set; }
    public Parent Parent { get; set; } = null!;
    public string RelationshipWithParent { get; set; } = string.Empty;
    public DateOnly DateOfBirth { get; set; }
    public string Gender { get; set; } = EGender.MALE;
    public string Address { get; set; } = string.Empty;
    public string Province { get; set; } = string.Empty;
    public string District { get; set; } = string.Empty;
    public string Ward { get; set; } = string.Empty;
    public ICollection<ViolationStudent> ViolationStudents { get; set; } = new List<ViolationStudent>();
}