using eBoardAPI.Common;
using eBoardAPI.Entities;

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
    public Task<IEnumerable<ScoreSheet>> GetScoreSheetsByClassAndSemesterAsync(Guid classId, int semester);
}