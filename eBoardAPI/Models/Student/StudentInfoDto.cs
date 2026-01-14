using eBoardAPI.Models.Parent;

namespace eBoardAPI.Models.Student;

public class StudentInfoDto
{
    public Guid Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string FullAddress { get; set; }
    public ParentInfoDto? Parent { get; set; }
    public required string RelationshipWithParent { get; set; }
    public DateOnly DateOfBirth { get; set; }
    public required string Gender { get; set; }
}