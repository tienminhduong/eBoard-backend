namespace eBoardAPI.Models.Activity;

public class ActivitySignInDto
{
    public Guid Id { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public string ActivityName { get; set; } = string.Empty;
    public DateTime SignInTime { get; set; }
    public string Status { get; set; } = string.Empty;
}