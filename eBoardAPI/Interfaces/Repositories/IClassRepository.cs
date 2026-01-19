using eBoardAPI.Common;
using eBoardAPI.Entities;

namespace eBoardAPI.Interfaces.Repositories;

public interface IClassRepository
{
    Task<IEnumerable<Grade>> GetAllGradesAsync();
    Task<IEnumerable<Class>> GetAllTeachingClassByTeacherAsync(Guid teacherId);
    Task<IEnumerable<Class>> GetAllClassesByTeacherAsync(Guid teacherId, int pageNumber, int pageSize);
    Task<IEnumerable<Student>> GetStudentsByClassAsync(Guid classId, int pageNumber, int pageSize);
    Task<Result<Class>> GetClassByIdAsync(Guid classId);
    Task<bool> ClassExistsAsync(Guid classId);
    void UpdateClass(Class @class);
    
    Task<Class> AddNewClassAsync(Class newClass);
    Task<Result> AddNewStudentsToClassAsync(Guid classId, List<Guid> studentIds);
    Task<bool> IsStudentInClassAsync(Guid classId, Guid studentId);
    Task<IEnumerable<Guid>> ValidateStudentsInClassAsync(Guid classId, IEnumerable<Guid> studentIds);
    Task<Result> RemoveStudentFromClassAsync(Guid classId, Guid studentId);
}