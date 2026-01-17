using AutoMapper;
using eBoardAPI.Common;
using eBoardAPI.Entities;
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
        
        // if (scoreSheet == null)
        // {
        //     var validate = await unitOfWork.ClassRepository.ClassExistsAsync(classId);
        //     if (!validate)
        //         return Result<StudentScoreSheetDto>.Failure("Lớp học không tồn tại");
        //     
        //     validate = await unitOfWork.StudentRepository.StudentExistsAsync(studentId);
        //     if (!validate)
        //         return Result<StudentScoreSheetDto>.Failure("Học sinh không tồn tại");
        //     
        //     scoreSheet = new ScoreSheet
        //     {
        //         StudentId = studentId,
        //         ClassId = classId,
        //         Semester = semester
        //     };
        //     scoreSheet = await unitOfWork.ScoreRepository.AddScoreSheetAsync(scoreSheet);
        //     await unitOfWork.SaveChangesAsync();
        // }
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
        
        await unitOfWork.SaveChangesAsync();

        var resultDto = mapper.Map<StudentScoreSheetDto>(scoreSheet);
        return (Result<StudentScoreSheetDto>.Success(resultDto), isCreated);
    }

    public async Task<IEnumerable<SubjectDto>> GetClassSubjectsAsync(Guid classId)
    {
        var subjects = await unitOfWork.SubjectRepository.GetAllSubjectsByClassAsync(classId);
        return mapper.Map<IEnumerable<SubjectDto>>(subjects);
    }
}