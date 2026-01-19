using eBoardAPI.Common;
using eBoardAPI.Entities;
using eBoardAPI.Models;
using eBoardAPI.Models.Class;
using eBoardAPI.Models.Student;
using eBoardAPI.Models.Subject;

namespace eBoardAPI.Interfaces.Services;

public interface IClassService
{
    Task<IEnumerable<ClassInfoDto>> GetAllTeachingClassesByTeacherAsync(Guid teacherId);
    Task<IEnumerable<Grade>> GetAllGradesAsync();
    Task<Result<ClassInfoDto>> GetClassByIdAsync(Guid classId);
    Task<PagedStudentInClassDto> GetPagedStudentsByClassAsync(Guid classId, int pageNumber, int pageSize);
    Task<PagedDto<ClassInfoDto>> GetPagedClassesByTeacherAsync(Guid teacherId, int pageNumber, int pageSize);
    
    Task<Result<ClassInfoDto>> AddNewClassAsync(CreateClassDto createClassDto, Guid teacherId);
    Task<IEnumerable<SubjectDto>> GetSubjectInClassAsync(Guid classId);
    Task<Result> RemoveStudentFromClassAsync(Guid classId, Guid studentId);
}