using eBoardAPI.Consts;

namespace eBoardAPI.Entities;

public class ActivitySignIn
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public Guid StudentId { get; set; }
    public Student Student { get; set; } = null!;
    public Guid ActivityId { get; set; }
    public ExtracurricularActivity Activity { get; set; } = null!;
    public DateTime SignInTime { get; set; }
    public string Status { get; set; } = EActivitySignInStatus.PENDING;
}