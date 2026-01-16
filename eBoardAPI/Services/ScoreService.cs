using AutoMapper;
using eBoardAPI.Interfaces.Repositories;
using eBoardAPI.Interfaces.Services;
using eBoardAPI.Models.ScoreSheet;

namespace eBoardAPI.Services;

public class ScoreService(
    IScoreRepository scoreRepository,
    IMapper mapper
    ) : IScoreService
{
    public async Task<ClassScoreSummaryDto> GetClassScoreSummaryAsync(Guid classId, int semester)
    {
        var (excellentCount, goodCount, averageCount, poorCount) =
            await scoreRepository.CountClassGradeAsync(classId, semester);

        var scoreSheets = await scoreRepository.GetScoreSheetsByClassAndSemesterAsync(classId, semester);
        
        var result = new ClassScoreSummaryDto
        {
            ExcellentCount = excellentCount,
            GoodCount = goodCount,
            AverageCount = averageCount,
            PoorCount = poorCount,
            StudentScores = mapper.Map<IEnumerable<StudentScoreSummaryDto>>(scoreSheets)
        };
        
        return result;
    }
}