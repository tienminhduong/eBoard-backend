using eBoardAPI.Entities;

namespace eBoardAPI.Models.Violation
{
    public class UpdateViolationDto
    {
        public required List<Guid> StudentIds { get; set; }
        public required Guid ClassId { get; set; }
        public required string InChargeTeacherName { get; set; }
        public required DateOnly ViolateDate { get; set; }
        public required string ViolationType { get; set; }
        public required ViolationLevel ViolationLevel { get; set; }
        public required string ViolationInfo { get; set; } = string.Empty;
        public required string Penalty { get; set; } = string.Empty;
    }
}
