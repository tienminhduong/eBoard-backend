namespace eBoardAPI.Entities;

public class ActivityParticipant
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public Guid StudentId { get; set; }
    public Student Student { get; set; } = null!;
    public Guid ActivityId { get; set; }
    public ExtracurricularActivity Activity { get; set; } = null!;
    public string ParentPhoneNumber { get; set; } = string.Empty;
    public string TeacherComments { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
}