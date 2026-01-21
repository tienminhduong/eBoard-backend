using eBoardAPI.Entities;
namespace eBoardAPI.Models.ExamSchedule
{
    public class ExamScheduleDto
    {
        public Guid Id { get; set; }
        public Guid SubjectId { get; set; }
        public eBoardAPI.Entities.Subject Subject { get; set; } = null!;
        public string ExamFormat { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public DateTime StartTime { get; set; }
        public string Notes { get; set; } = string.Empty;
    }
}
