using eBoardAPI.Common;
using eBoardAPI.Entities;

namespace eBoardAPI.Interfaces.Repositories;

public interface ISubjectRepository
{
    Task<Result<Subject>> GetSubjectByIdAsync(Guid subjectId);
    Task<Result<Subject>> GetSubjectByNameAsync(string subjectName);
    Task<Subject> GetOrAddSubjectByNameAsync(string subjectName);
    Task<Subject> AddSubjectAsync(Subject subject);
    Task<IEnumerable<Subject>> GetAllSubjectsAsync();
    Task<IEnumerable<Subject>> GetAllSubjectsByClassAsync(Guid classId);
}