using eBoardAPI.Common;
using eBoardAPI.Entities;

namespace eBoardAPI.Interfaces.Repositories;

public interface ITeacherRepository
{
    Task<Result<Teacher>> GetByIdAsync(Guid id);
    Task<Result<Teacher>> UpdateAsync(Teacher teacher);
    Task<bool> EmailExistsAsync(string email);
    Task<bool> PhoneExistsAsync(string phone);
    Task<Result> AddAsync(Teacher teacher);
}