using eBoardAPI.Common;
using eBoardAPI.Models.ScoreSheet;
using eBoardAPI.Models.Subject;

namespace eBoardAPI.Interfaces.Services;

public interface IScoreService
{
    Task<ClassScoreSummaryDto> GetClassScoreSummaryAsync(Guid classId, int semester);
    Task<Result<StudentScoreSheetDto>> GetStudentScoreSheetAsync(Guid classId, Guid studentId, int semester);
    Task<(Result<StudentScoreSheetDto>, bool isCreated)> AddOrUpdateStudentScoreSheetAsync(Guid classId, Guid studentId,
        int semester,
        UpdateIndividualStudentScoreSheetDto updateDto);
    Task<IEnumerable<SubjectDto>> GetClassSubjectsAsync(Guid classId);
    Task<IEnumerable<StudentScoreBySubjectDto>> GetStudentScoreBySubjectsAsync(Guid classId, Guid subjectId, int semester);
}