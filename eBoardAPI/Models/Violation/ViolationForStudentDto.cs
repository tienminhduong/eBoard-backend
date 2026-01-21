using eBoardAPI.Entities;

namespace eBoardAPI.Models.Violation
{
    public class ViolationForStudentDto
    {
        public Guid Id { get; set; }
        public required string InChargeTeacherName { get; set; }
        public DateOnly ViolateDate { get; set; }
        public string ViolationType { get; set; } = string.Empty;
        public ViolationLevel ViolationLevel { get; set; }
        public string ViolationInfo { get; set; } = string.Empty;
        public string Penalty { get; set; } = string.Empty;
        public bool SeenByParent { get; set; }
    }
}
