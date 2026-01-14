using eBoardAPI.Common;
using eBoardAPI.Entities;

namespace eBoardAPI.Interfaces.Repositories;

public interface IClassRepository
{
    Task<IEnumerable<Grade>> GetAllGradesAsync();
    Task<IEnumerable<Class>> GetAllTeachingClassByTeacherAsync(Guid teacherId);
    Task<IEnumerable<Class>> GetAllClassesByTeacherAsync(Guid teacherId, int pageNumber, int pageSize);
    Task<IEnumerable<Student>> GetAllStudentsByClassAsync(Guid classId, int pageNumber, int pageSize);
    Task<Result<Class>> GetClassByIdAsync(Guid classId);
    
    Task<Result> AddNewClassAsync(Class newClass);
}