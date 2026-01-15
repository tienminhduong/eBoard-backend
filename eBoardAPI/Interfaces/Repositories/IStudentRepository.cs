using eBoardAPI.Common;
using eBoardAPI.Entities;

namespace eBoardAPI.Interfaces.Repositories;

public interface IStudentRepository
{
    Task<Result<Student>> GetByIdAsync(Guid id);
    Task<Student> AddAsync(Student student);
}