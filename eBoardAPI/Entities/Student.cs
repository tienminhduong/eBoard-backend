namespace eBoardAPI.Entities;


public class Student
{
    public Guid Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public string Address { get; set; } = "";
    public Guid? ParentId { get; set; }
    public Parent? Parent { get; set; }
    public string RelationshipWithParent { get; set; } = "";
    public DateOnly DateOfBirth { get; set; }
    public string Gender { get; set; } = "";
    public Guid? ProvinceId { get; set; }
    public Province? Province { get; set; }
    public Guid? DistrictId { get; set; }
    public District? District { get; set; }
    public Guid? WardId { get; set; }
    public Ward? Ward { get; set; }
}