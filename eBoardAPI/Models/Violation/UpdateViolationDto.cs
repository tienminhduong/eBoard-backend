using eBoardAPI.Entities;

namespace eBoardAPI.Models.Violation
{
    public class UpdateViolationDto
    {
        public List<Guid> StudentId { get; set; } = null!;
        public Guid ClassId { get; set; }
        public string InChargeTeacherName { get; set; } = null!;
        public DateOnly? ViolateDate { get; set; } = null!;
        public string ViolationType { get; set; } = null!;
        public ViolationLevel? ViolationLevel { get; set; } = null!;
        public string ViolationInfo { get; set; } = null!;
        public string Penalty { get; set; } = null!;
    }
}
