using eBoardAPI.Entities;
using eBoardAPI.Models.Class;

namespace eBoardAPI.Interfaces.Services;

public interface IClassService
{
    Task<IEnumerable<ClassInfoDto>> GetAllTeachingClassesByTeacher(Guid teacherId);
}