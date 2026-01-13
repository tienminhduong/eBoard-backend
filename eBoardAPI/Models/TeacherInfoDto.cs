namespace eBoardAPI.Models;

public class TeacherInfoDto
{
    public required Guid Id { get; set; }
    public required string FullName { get; set; }
    public required string Email { get; set; }
    public required string PhoneNumber { get; set; }
    public required string Qualifications { get; set; }
}