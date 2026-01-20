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
        public Guid ClassId { get; set; }
        public Class Class { get; set; } = null!;
        public required string InChargeTeacherName { get; set; }
        public DateOnly ViolateDate { get; set; }
        public string ViolationType { get; set; } = string.Empty;
        public ViolationLevel ViolationLevel { get; set; }
        public string ViolationInfo { get; set; } = string.Empty;
        public string Penalty { get; set; } = string.Empty;
        public bool SeenByParent { get; set; } = false;
        public ICollection<ViolationStudent> Students { get; set; } = new List<ViolationStudent>();
    }
}
