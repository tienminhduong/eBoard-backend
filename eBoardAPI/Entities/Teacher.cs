namespace eBoardAPI.Entities;

public class Teacher
{
    public Guid Id { get; set; }
    public required string PasswordHash { get; set; }
    public required string FullName { get; set; }
    public required string Email { get; set; }
    public required string PhoneNumber { get; set; }
    public required string Qualifications { get; set; }
}