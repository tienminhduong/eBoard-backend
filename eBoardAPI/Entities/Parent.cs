namespace eBoardAPI.Entities;

public class Parent
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public required string FullName { get; set; }
    public required string PhoneNumber { get; set; }
    public string Email { get; set; } = string.Empty;
    public string GeneratedPassword { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string HealthCondition { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
}
