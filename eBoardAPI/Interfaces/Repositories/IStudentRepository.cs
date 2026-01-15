using eBoardAPI.Entities;

namespace eBoardAPI.Interfaces.Repositories;

public interface IStudentRepository
{
    Task<Student?> GetByIdAsync(Guid id);
    Task<bool> AddAsync(Student student);
}