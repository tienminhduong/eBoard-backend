namespace eBoardAPI.Entities;

public class Parent
{
    public Guid Id { get; set; }
    public required string FullName { get; set; }
    public required string PhoneNumber { get; set; }
    public string Email { get; set; } = "";
    public string GeneratedPassword { get; set; } = "";
    public string PasswordHash { get; set; } = "";
    public string HealthCondition { get; set; } = "";
    public string Address { get; set; } = "";
}
