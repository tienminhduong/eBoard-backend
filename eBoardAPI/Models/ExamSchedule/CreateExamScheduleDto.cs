namespace eBoardAPI.Models.ExamSchedule
{
    public class CreateExamScheduleDto
    {
        public required Guid SubjectId { get; set; }
        public required string ExamFormat { get; set; }
        public required string Location { get; set; }
        public required DateTime StartTime { get; set; }
        public string Notes { get; set; } = string.Empty;
    }
}
