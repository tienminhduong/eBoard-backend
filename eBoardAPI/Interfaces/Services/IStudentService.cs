using eBoardAPI.Models.Student;

namespace eBoardAPI.Interfaces.Services;

public interface IStudentService
{
    Task<StudentInfoDto?> GetByIdAsync(Guid id);
    Task<StudentInfoDto> CreateAsync(CreateStudentDto student);
}