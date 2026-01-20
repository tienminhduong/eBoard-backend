using eBoardAPI.Common;
using eBoardAPI.Entities;

namespace eBoardAPI.Interfaces.Repositories;

public interface IStudentRepository
{
    Task<Result<Student>> GetByIdAsync(Guid id);
    Task<Student> AddAsync(Student student);
    Task<bool> StudentExistsAsync(Guid studentId);
    Task<IEnumerable<Tuple<Guid, string>>> GetStudentsOptionInClassAsync(Guid classId);
    void UpdateStudent(Student student);
    Task<Result<Parent>> GetParentByStudentIdAsync(Guid studentId);
}