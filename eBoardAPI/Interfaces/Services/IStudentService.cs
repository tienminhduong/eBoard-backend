using eBoardAPI.Common;
using eBoardAPI.Models.Student;

namespace eBoardAPI.Interfaces.Services;

public interface IStudentService
{
    Task<Result<StudentInfoDto>> GetByIdAsync(Guid id);
    Task<Result<StudentInfoDto>> CreateAsync(CreateStudentDto student);
}