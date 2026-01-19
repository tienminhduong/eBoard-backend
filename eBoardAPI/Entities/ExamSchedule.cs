namespace eBoardAPI.Entities
{
    public class ExamSchedule
    {
        public Guid Id { get; set; }
        public Guid SubjectId { get; set; }
        public Subject Subject { get; set; } = null!;
        public Guid ClassId { get; set; }
        public Class Class { get; set; } = null!;
        public required string ExamFormat { get; set; }
        public required string Location { get; set; }
        public DateTime StartTime { get; set; }
        public string Notes { get; set; } = string.Empty;
    }
}
