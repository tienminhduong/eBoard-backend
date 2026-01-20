using eBoardAPI.Common;
using eBoardAPI.Entities;

namespace eBoardAPI.Interfaces.Repositories
{
    public interface IViolationRepository
    {
        Task<Result<IEnumerable<Violation>>> AddRangeAsync(IEnumerable<Violation> violations);
        Task<Result<IEnumerable<Violation>>> GetRangeByIdsAsync(IEnumerable<Guid> ids);
        Task<Result<Violation>> AddAsync(Violation violation);
        Task<Result> AddRangeViolationStudentsAsync(IEnumerable<ViolationStudent> violationStudents);
        Task<Result<Violation>> GetByIdAsync(Guid id);
        Task<Result<IEnumerable<ViolationStudent>>> GetViolationStudentByViolationIdAsync(Guid violationId);
        Task<Result> RemoveRangeViolationStudentsAsync(IEnumerable<ViolationStudent> violationStudents);
        Task<Result> UpdateAsync(Violation violation);
        Task<Result> UpdateAndSaveAsync(Violation violation);
        Task<Result<ViolationStudent>> GetViolationStudentByIds(Guid violationId, Guid studentId);
        Task<Result> UpdateAsync(ViolationStudent violationStudent);
    }
}
