using eBoardAPI.Common;
using eBoardAPI.Models.Student;

namespace eBoardAPI.Interfaces.Services;

public interface IStudentService
{
    Task<Result<StudentInfoDto>> GetByIdAsync(Guid id);
    Task<Result<StudentInfoDto>> AddNewStudentAsync(CreateStudentDto createStudentDto);
    Task<IEnumerable<Tuple<Guid, string>>> GetStudentsOptionInClassAsync(Guid classId);
}