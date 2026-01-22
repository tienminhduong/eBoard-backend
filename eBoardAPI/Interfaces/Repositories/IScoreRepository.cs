using eBoardAPI.Common;
using eBoardAPI.Entities;
using eBoardAPI.Models.ScoreSheet;

namespace eBoardAPI.Interfaces.Repositories;

public interface IScoreRepository
{
    public Task<(
        int excellentCount,
        int goodCount,
        int averageCount,
        int poorCount
        )> CountClassGradeAsync(Guid classId, int semester);
    
    public Task<Result<ScoreSheet>> GetScoreSheetByIdAsync(Guid scoreSheetId);
    public Task<IEnumerable<ScoreSheet>> GetScoreSheetsByClassAndSemesterAsync(Guid classId, int semester,
        bool includeStudent = true, bool includeClass = true);
    public Task EvaluateClassRankAsync(Guid classId, int semester);
    public Task<IEnumerable<ScoreSheetDetail>> GetScoreSheetDetailsBySubjectInClassAsync(Guid classId, Guid subjectId, int semester);
    public Task<ScoreSheet?> GetStudentScoreSheetAsync(Guid classId, Guid studentId, int semester);
    public void UpdateScoreSheetDetailsAsync(ScoreSheetDetail scoreSheetDetail);
    public Task<ScoreSheet> AddScoreSheetAsync(ScoreSheet scoreSheet);
    
    public Task<IEnumerable<StudentScoreBySubjectDto>> GetScoresBySubjectAsync(Guid classId, Guid subjectId, int semester);
    Task AddScoreSheetDetailAsync(ScoreSheetDetail scoreSheetDetail);
    void UpdateScoreSheetAsync(ScoreSheet scoreSheet);
}