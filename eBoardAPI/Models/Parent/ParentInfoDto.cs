namespace eBoardAPI.Models.Parent;

public class ParentInfoDto
{
    public required Guid Id { get; set; }
    public required string FullName { get; set; }
    public required string Email { get; set; }
    public required string PhoneNumber { get; set; }
    public required string Address { get; set; }
    public string HealthCondition { get; set; } = string.Empty;
}