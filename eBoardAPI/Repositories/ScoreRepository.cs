using eBoardAPI.Common;
using eBoardAPI.Context;
using eBoardAPI.Entities;
using eBoardAPI.Interfaces.Repositories;
using eBoardAPI.Models.ScoreSheet;
using Microsoft.EntityFrameworkCore;

namespace eBoardAPI.Repositories;

public class ScoreRepository(AppDbContext dbContext) : IScoreRepository
{
    public async Task<(
        int excellentCount,
        int goodCount,
        int averageCount,
        int poorCount
        )> CountClassGradeAsync(Guid classId, int semester)
    {
        var query = from scoreSheet in dbContext.ScoreSheets
            where scoreSheet.ClassId == classId && scoreSheet.Semester == semester
            orderby scoreSheet.AverageScore
            select scoreSheet;
        
        var excellentCount = await query.CountAsync(s => s.AverageScore >= 9f);
        var goodCount = await query.CountAsync(s => s.AverageScore >= 8f && s.AverageScore < 9f);
        var averageCount = await query.CountAsync(s => s.AverageScore >= 5f && s.AverageScore < 8f);
        var poorCount = await query.CountAsync(s => s.AverageScore < 5f);
        
        return (excellentCount, goodCount, averageCount, poorCount);
    }

    public async Task<Result<ScoreSheet>> GetScoreSheetByIdAsync(Guid scoreSheetId)
    {
        var scoreSheet = await dbContext.ScoreSheets
            .Include(s => s.Student)
            .Include(s => s.Class)
            .Include(s => s.Details)
            .FirstOrDefaultAsync(s => s.Id == scoreSheetId);
        
        return scoreSheet == null
            ? Result<ScoreSheet>.Failure("Không tìm thấy bảng điểm")
            : Result<ScoreSheet>.Success(scoreSheet);
    }

    public async Task<IEnumerable<ScoreSheet>> GetScoreSheetsByClassAndSemesterAsync(Guid classId, int semester,
        bool includeStudent = true, bool includeClass = true)
    {
        var query = from scoreSheet in dbContext.ScoreSheets
            join student in dbContext.Students on scoreSheet.StudentId equals student.Id
            where scoreSheet.ClassId == classId && scoreSheet.Semester == semester
            orderby student.FirstName
            select scoreSheet;
        
        if (includeStudent)
            query = query.Include(s => s.Student);
        if (includeClass)
            query = query.Include(s => s.Class);
        
        return await query.ToListAsync();
    }

    public async Task EvaluateClassRankAsync(Guid classId, int semester)
    {
        var scoreSheets = await GetScoreSheetsByClassAndSemesterAsync(classId, semester);
        var rankedSheets = scoreSheets
            .OrderByDescending(s => s.AverageScore)
            .ToList();
        
        for (var i = 0; i < rankedSheets.Count; i++)
        {
            rankedSheets[i].Rank = i + 1;
            dbContext.ScoreSheets.Update(rankedSheets[i]);
        }
    }

    public async Task<IEnumerable<ScoreSheetDetail>> GetScoreSheetDetailsBySubjectInClassAsync(Guid classId, Guid subjectId, int semester)
    {
        var query = from scoreSheet in dbContext.ScoreSheets
            join scoreDetail in dbContext.ScoreSheetDetails on scoreSheet.Id equals scoreDetail.ScoreSheetId
            where scoreSheet.ClassId == classId
                  && scoreSheet.Semester == semester
                  && scoreDetail.SubjectId == subjectId
            select scoreDetail;
        
        return await query.ToListAsync();
    }

    public async Task<ScoreSheet?> GetStudentScoreSheetAsync(Guid classId, Guid studentId, int semester)
    {
        var query = from scoreSheet in dbContext.ScoreSheets
            where scoreSheet.ClassId == classId
                  && scoreSheet.StudentId == studentId
                  && scoreSheet.Semester == semester
            select scoreSheet;
        
        var sheet = await query
            .Include(s => s.Student)
            .Include(s => s.Class)
            .Include(s => s.Details).ThenInclude(d => d.Subject)
            .FirstOrDefaultAsync();
        
        return sheet;
    }

    public void UpdateScoreSheetDetailsAsync(ScoreSheetDetail scoreSheetDetail)
    {
        dbContext.ScoreSheetDetails.Update(scoreSheetDetail);
    }

    public async Task<ScoreSheet> AddScoreSheetAsync(ScoreSheet scoreSheet)
    {
        await dbContext.ScoreSheets.AddAsync(scoreSheet);
        return scoreSheet;
    }

    public async Task<IEnumerable<StudentScoreBySubjectDto>> GetScoresBySubjectAsync(Guid classId, Guid subjectId, int semester)
    {
        var query = from scoreSheet in dbContext.ScoreSheets
            join scoreDetail in dbContext.ScoreSheetDetails on scoreSheet.Id equals scoreDetail.ScoreSheetId
            join student in dbContext.Students on scoreSheet.StudentId equals student.Id
            where scoreSheet.ClassId == classId
                  && scoreSheet.Semester == semester
                  && scoreDetail.SubjectId == subjectId
            orderby student.FirstName
            select new StudentScoreBySubjectDto
            {
                StudentId = student.Id,
                StudentName = student.LastName + " " + student.FirstName,
                MidtermScore = scoreDetail.MidtermScore,
                FinalScore = scoreDetail.FinalScore,
                AverageScore = scoreDetail.AverageScore,
                Grade = scoreSheet.Grade
            };
        
        return await query.AsNoTracking().ToListAsync();
    }

    public async Task AddScoreSheetDetailAsync(ScoreSheetDetail scoreSheetDetail)
    {
        await dbContext.ScoreSheetDetails.AddAsync(scoreSheetDetail);
    }
}