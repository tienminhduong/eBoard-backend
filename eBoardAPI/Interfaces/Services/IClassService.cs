using eBoardAPI.Common;
using eBoardAPI.Entities;
using eBoardAPI.Models.Class;
using eBoardAPI.Models.Student;

namespace eBoardAPI.Interfaces.Services;

public interface IClassService
{
    Task<IEnumerable<ClassInfoDto>> GetAllTeachingClassesByTeacherAsync(Guid teacherId);
    Task<IEnumerable<Grade>> GetAllGradesAsync();
    Task<Result<ClassInfoDto>> GetClassByIdAsync(Guid classId);
    Task<PagedStudentInClassDto> GetPagedStudentsByClassAsync(Guid classId, int pageNumber, int pageSize);
    
    Task<Result<ClassInfoDto>> AddNewClassAsync(CreateClassDto createClassDto, Guid teacherId);
}