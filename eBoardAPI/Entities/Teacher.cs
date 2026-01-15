namespace eBoardAPI.Entities;

public class Teacher
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public required string PasswordHash { get; set; }
    public string ProfileImgUrl { get; set; } = string.Empty;
    public required string FullName { get; set; }
    public required string Email { get; set; }
    public string PhoneNumber { get; set; } = string.Empty;
    public string Qualifications { get; set; } = string.Empty;
}