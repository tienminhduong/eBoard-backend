namespace eBoardAPI.Models.ExamSchedule
{
    public class ExamScheduleFilter
    {
        // filter time
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }

        // filter by Id
        public Guid? SubjectId { get; set; }

        // filter by exam format
        public string? ExamFormat { get; set; }

        // filter by pagination
        public int PageNumber = 1;
        public int PageSize = 10;
    }
}
