using eBoardAPI.Entities;

namespace eBoardAPI.Models.Violation
{
    public class ViolationDto
    {
        public Guid Id { get; set; }
        public IEnumerable<IdStudentPair> InvolvedStudents { get; set; } = new List<IdStudentPair>();
        public Guid ClassId { get; set; }
        public required string InChargeTeacherName { get; set; }
        public DateOnly ViolateDate { get; set; }
        public string ViolationType { get; set; } = string.Empty;
        public ViolationLevel ViolationLevel { get; set; }
        public string ViolationInfo { get; set; } = string.Empty;
        public string Penalty { get; set; } = string.Empty;
    }
    public class IdStudentPair
    {
        public Guid StudentId { get; set; }
        public string StudentName { get; set; } = string.Empty;
    }
}
