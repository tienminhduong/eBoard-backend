using Microsoft.EntityFrameworkCore;

namespace eBoardAPI.Entities
{
    [PrimaryKey(nameof(ViolationId), nameof(StudentId))]
    public class ViolationStudent
    {
        public Guid ViolationId { get; set; }
        public Violation Violation { get; set; } = null!;
        public Guid StudentId { get; set; }
        public Student Student { get; set; } = null!;
        public bool SeenByParent { get; set; } = false;
    }
}
