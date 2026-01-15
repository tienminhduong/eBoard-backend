using eBoardAPI.Common;
using eBoardAPI.Entities;

namespace eBoardAPI.Interfaces.Repositories;

public interface ITeacherRepository
{
    Task<Result<Teacher>> GetByIdAsync(Guid id);
    Task<Result<Teacher>> UpdateAsync(Teacher teacher);
}