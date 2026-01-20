namespace eBoardAPI.Models.ExamSchedule
{
    public class ExamScheduleFilter
    {
        // filter time
        public DateTime? From;
        public DateTime? To;

        // filter by Id
        public Guid? SubjectId;

        // filter by exam format
        public string? ExamFormat;

        // filter by pagination
        public int PageNumber = 1;
        public int PageSize = 10;
    }
}
