namespace eBoardAPI.Models.Student;

public class PagedStudentInClassDto
{
    public IEnumerable<StudentInfoDto> Data { get; set; }
    public int PageSize { get; set; }
    public int PageNumber { get; set; }
    public int TotalRecords { get; set; }
    public Guid ClassId { get; set; }
}