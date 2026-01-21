using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using eBoardAPI.Common;
using eBoardAPI.Entities;
using eBoardAPI.Helpers;
using eBoardAPI.Interfaces.Repositories;
using eBoardAPI.Interfaces.Services;
using eBoardAPI.Models.ScoreSheet;
using eBoardAPI.Models.Subject;

namespace eBoardAPI.Services;

public class ScoreService(
    IScoreRepository scoreRepository,
    IUnitOfWork unitOfWork,
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

    public async Task<Result<StudentScoreSheetDto>> GetStudentScoreSheetAsync(Guid classId, Guid studentId,
        int semester)
    {
        var scoreSheet = await unitOfWork.ScoreRepository.GetStudentScoreSheetAsync(classId, studentId, semester);
        return scoreSheet == null
            ? Result<StudentScoreSheetDto>.Failure("Bảng điểm không tồn tại")
            : Result<StudentScoreSheetDto>.Success(mapper.Map<StudentScoreSheetDto>(scoreSheet));
    }

    public async Task<(Result<StudentScoreSheetDto>, bool isCreated)> AddOrUpdateStudentScoreSheetAsync(Guid classId,
        Guid studentId, int semester,
        UpdateIndividualStudentScoreSheetDto updateDto)
    {
        var isCreated = false;
        var scoreSheet = await unitOfWork.ScoreRepository.GetStudentScoreSheetAsync(classId, studentId, semester);
        if (scoreSheet == null)
        {
            scoreSheet = new ScoreSheet
            {
                StudentId = studentId,
                ClassId = classId,
                Semester = semester
            };
            scoreSheet = await unitOfWork.ScoreRepository.AddScoreSheetAsync(scoreSheet);
            isCreated = true;
        }
        
        // update score detail here.
        foreach (var detailDto in updateDto.SubjectScores)
        {
            var detail = scoreSheet.Details.FirstOrDefault(s => s.SubjectId == detailDto.SubjectId);
            if (detail == null)
            {
                scoreSheet.Details.Add(new ScoreSheetDetail
                {
                    ScoreSheetId =  scoreSheet.Id,
                    SubjectId =  detailDto.SubjectId,
                    MidtermScore =  detailDto.MidtermScore,
                    FinalScore =  detailDto.FinalScore,
                    AverageScore = (detailDto.MidtermScore + detailDto.FinalScore) / 2
                });
            }
            else
            {
                detail.MidtermScore = detailDto.MidtermScore;
                detail.FinalScore = detailDto.FinalScore;
                detail.AverageScore = (detailDto.MidtermScore + detailDto.FinalScore) / 2;
                unitOfWork.ScoreRepository.UpdateScoreSheetDetailsAsync(detail);
            }
        }
        
        scoreSheet.AverageScore = scoreSheet.Details.Any()
            ? scoreSheet.Details.Average(d => d.AverageScore)
            : 0;
        
        scoreSheet.Grade = StringHelper.ScoreToGrade(scoreSheet.AverageScore);
        await unitOfWork.ScoreRepository.EvaluateClassRankAsync(classId, semester);
        
        await unitOfWork.SaveChangesAsync();

        var resultDto = mapper.Map<StudentScoreSheetDto>(scoreSheet);
        return (Result<StudentScoreSheetDto>.Success(resultDto), isCreated);
    }

    [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
    public async Task<bool> UpdateScoresBySubjectAsync(Guid classId, Guid subjectId, int semester, IEnumerable<UpdateStudentScoreBySubjectDto> updateDtos)
    {
        var studentIds = updateDtos.Select(d => d.StudentId);
        await CreateScoreSheetsIfNotExists(classId, semester, studentIds);
        
        var existingSheets = await unitOfWork.ScoreRepository.GetScoreSheetsByClassAndSemesterAsync(classId, semester,
            includeStudent: false, includeClass: false);
        var scoreSheetDict = existingSheets.ToDictionary(s => s.StudentId, s => s.Id);
        
        var existingDetails = await unitOfWork.ScoreRepository.GetScoreSheetDetailsBySubjectInClassAsync(classId, subjectId, semester);
        var detailDict = existingDetails.ToDictionary(d => d.ScoreSheetId);
        
        foreach (var updateDto in updateDtos)
        {
            if (!scoreSheetDict.TryGetValue(updateDto.StudentId, out var scoreSheetId))
                continue;
            
            if (detailDict.TryGetValue(scoreSheetId, out var detail))
            {
                detail.MidtermScore = updateDto.MidtermScore;
                detail.FinalScore = updateDto.FinalScore;
                detail.AverageScore = (updateDto.MidtermScore + updateDto.FinalScore) / 2;
                unitOfWork.ScoreRepository.UpdateScoreSheetDetailsAsync(detail);
            }
            else
            {
                var newDetail = new ScoreSheetDetail
                {
                    ScoreSheetId = scoreSheetId,
                    SubjectId = subjectId,
                    MidtermScore = updateDto.MidtermScore,
                    FinalScore = updateDto.FinalScore,
                    AverageScore = (updateDto.MidtermScore + updateDto.FinalScore) / 2
                };
                await unitOfWork.ScoreRepository.AddScoreSheetDetailAsync(newDetail);
            }
        }
        
        await unitOfWork.ScoreRepository.EvaluateClassRankAsync(classId, semester);

        try
        {
            await unitOfWork.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<IEnumerable<SubjectDto>> GetClassSubjectsAsync(Guid classId)
    {
        var subjects = await unitOfWork.SubjectRepository.GetAllSubjectsByClassAsync(classId);
        return mapper.Map<IEnumerable<SubjectDto>>(subjects);
    }

    public async Task<IEnumerable<StudentScoreBySubjectDto>> GetStudentScoreBySubjectsAsync(Guid classId, Guid subjectId, int semester)
    {
        return await unitOfWork.ScoreRepository.GetScoresBySubjectAsync(classId, subjectId, semester);
    }

    private async Task CreateScoreSheetsIfNotExists(Guid classId, int semester, IEnumerable<Guid> studentIds)
    {
        var existingSheets = await unitOfWork.ScoreRepository.GetScoreSheetsByClassAndSemesterAsync(classId, semester);
        var existingStudentIds = existingSheets.Select(s => s.StudentId).ToHashSet();

        var notExistingStudentIds = studentIds.Except(existingStudentIds).ToList();
        var notExistingValidStudentIds = await unitOfWork.ClassRepository
            .ValidateStudentsInClassAsync(classId, notExistingStudentIds);
        
        foreach (var scoreSheet in notExistingValidStudentIds.Select(studentId => new ScoreSheet
                 {
                     StudentId = studentId,
                     ClassId = classId,
                     Semester = semester
                 }))
        {
            await unitOfWork.ScoreRepository.AddScoreSheetAsync(scoreSheet);
        }
        await unitOfWork.SaveChangesAsync();
    }
}