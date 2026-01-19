namespace eBoardAPI.Entities
{
    public enum ViolationLevel
    {
        LOW,
        MEDIUM,
        HIGH
    }
    public class Violation
    {
        public Guid Id { get; set; }
        public Guid StudentId { get; set; }
        public Student Student { get; set; } = null!;
        public required string InChargeTeacherName { get; set; }
        public DateOnly ViolateDate { get; set; }
        public string ViolationType { get; set; } = string.Empty;
        public ViolationLevel ViolationLevel { get; set; }
        public string ViolationInfo { get; set; } = string.Empty;
        public string Penalty { get; set; } = string.Empty;
        public bool SeenByParent { get; set; }
    }
}
