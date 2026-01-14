namespace eBoardAPI.Models.Student;

public class PagedStudentInClassDto : PagedDto<StudentInfoDto>
{
    public Guid ClassId { get; set; }
}