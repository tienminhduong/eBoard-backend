namespace eBoardAPI.Entities;


public class Student
{
    public Guid Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public Guid? ParentId { get; set; }
    public Parent? Parent { get; set; }
    public string RelationshipWithParent { get; set; } = "";
    public DateOnly DateOfBirth { get; set; }
    public string Gender { get; set; } = "";
    public string Address { get; set; } = "";
    public string Province { get; set; } = "";
    public string District { get; set; } = "";
    public string Ward { get; set; } = "";
}