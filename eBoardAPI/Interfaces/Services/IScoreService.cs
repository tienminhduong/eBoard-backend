using eBoardAPI.Models.ScoreSheet;

namespace eBoardAPI.Interfaces.Services;

public interface IScoreService
{
    Task<ClassScoreSummaryDto> GetClassScoreSummaryAsync(Guid classId, int semester);
}