using System.Runtime.InteropServices.ComTypes;
using AutoMapper;
using eBoardAPI.Common;
using eBoardAPI.Entities;
using eBoardAPI.Interfaces.Repositories;
using eBoardAPI.Interfaces.Services;
using eBoardAPI.Models.Class;
using eBoardAPI.Models.Schedule;

namespace eBoardAPI.Services;

public class ScheduleService(
    IUnitOfWork unitOfWork,
    IMapper mapper
    ) : IScheduleService
{
    public async Task<Result<ClassPeriodDto>> AddClassPeriodAsync(CreateClassPeriodDto createClassPeriodDto)
    {
        try
        {
            var subject =
                await unitOfWork.SubjectRepository.GetOrAddSubjectByNameAsync(createClassPeriodDto.Subject.Name);
            var classPeriod = mapper.Map<ClassPeriod>(createClassPeriodDto);
            classPeriod.SubjectId = subject.Id;

            await unitOfWork.ScheduleRepository.AddClassPeriodAsync(classPeriod);
            await unitOfWork.SaveChangesAsync();

            classPeriod.Subject = subject;
            return Result<ClassPeriodDto>.Success(mapper.Map<ClassPeriodDto>(classPeriod));
        }
        catch (Exception ex)
        {
            return Result<ClassPeriodDto>.Failure($"Lỗi khi thêm tiết học: {ex.Message}");
        }
    }

    public async Task<Result<ClassPeriodDto>> UpdateClassPeriodAsync(Guid classPeriodId,
        UpdateClassPeriodDto updateClassPeriodDto)
    {
        var existingResult = await unitOfWork.ScheduleRepository.GetClassPeriodByIdAsync(classPeriodId);
        var subjectCheckTask = unitOfWork.SubjectRepository.GetOrAddSubjectByNameAsync(updateClassPeriodDto.Subject.Name);
        if (!existingResult.IsSuccess)
            return Result<ClassPeriodDto>.Failure(existingResult.ErrorMessage!);
        
        var existingClassPeriod = existingResult.Value!;
        
        existingClassPeriod.Notes = updateClassPeriodDto.Notes;
        existingClassPeriod.TeacherName = updateClassPeriodDto.TeacherName;
        existingClassPeriod.Subject = await subjectCheckTask;
        
        unitOfWork.ScheduleRepository.UpdateClassPeriod(existingClassPeriod);
        await unitOfWork.SaveChangesAsync();
        
        return Result<ClassPeriodDto>.Success(mapper.Map<ClassPeriodDto>(existingClassPeriod));
    }

    public async Task<bool> DeleteClassPeriodAsync(Guid classPeriodId)
    {
        var deleteResult = await unitOfWork.ScheduleRepository.DeleteClassPeriodAsync(classPeriodId);
        if (deleteResult)
            await unitOfWork.SaveChangesAsync();
        
        return deleteResult;
    }

    public async Task<Result<ScheduleDto>> GetClassPeriodsByClassAsync(Guid classId)
    {
        var classPeriods = await unitOfWork.ScheduleRepository.GetClassPeriodsByClassAsync(classId);
        var classPeriodDtos = mapper.Map<IEnumerable<ClassPeriodDto>>(classPeriods);
        
        var classInfo = await unitOfWork.ClassRepository.GetClassByIdAsync(classId);
        if (!classInfo.IsSuccess)
            return Result<ScheduleDto>.Failure(classInfo.ErrorMessage!);
        
        var returnDto = new ScheduleDto
        {
            Class = mapper.Map<ClassInfoDto>(classInfo.Value),
            ClassPeriods = classPeriodDtos.ToList()
        };
        
        return Result<ScheduleDto>.Success(returnDto);
    }
}