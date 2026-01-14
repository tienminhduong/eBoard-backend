namespace eBoardAPI.Entities;


public class Student
{
    public Guid Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public Guid ParentId { get; set; }
    public Parent Parent { get; set; } = null!;
    public string RelationshipWithParent { get; set; } = string.Empty;
    public DateOnly DateOfBirth { get; set; }
    public string Gender { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Province { get; set; } = string.Empty;
    public string District { get; set; } = string.Empty;
    public string Ward { get; set; } = string.Empty;
    
    public string GetFullAddress()
    {
        var address = Address;
        if (!string.IsNullOrEmpty(Ward))
        {
            address += $", {Ward}";
        }
        if (!string.IsNullOrEmpty(District))
        {
            address += $", {District}";
        }
        if (!string.IsNullOrEmpty(Province))
        {
            address += $", {Province}";
        }
        return address;
    }
}