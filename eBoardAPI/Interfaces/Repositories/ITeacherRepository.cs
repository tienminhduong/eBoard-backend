using eBoardAPI.Entities;

namespace eBoardAPI.Interfaces.Repositories;

public interface ITeacherRepository
{
    Task<Teacher?> GetByIdAsync(Guid id);
    Task<int> Update(Teacher teacher);
}