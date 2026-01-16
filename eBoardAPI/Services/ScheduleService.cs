using System.Runtime.InteropServices.ComTypes;
using AutoMapper;
using eBoardAPI.Common;
using eBoardAPI.Entities;
using eBoardAPI.Extensions;
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
        if (!existingResult.IsSuccess)
            return Result<ClassPeriodDto>.Failure(existingResult.ErrorMessage!);
        var existingClassPeriod = existingResult.Value!;

        if (updateClassPeriodDto.Subject != null)
        {
            var subject =
                await unitOfWork.SubjectRepository.GetOrAddSubjectByNameAsync(updateClassPeriodDto.Subject.Name);
            existingClassPeriod.SubjectId = subject.Id;
            existingClassPeriod.Subject = subject;
        }
        
        if (updateClassPeriodDto.Notes != null)
            existingClassPeriod.Notes = updateClassPeriodDto.Notes;
        
        if (updateClassPeriodDto.TeacherName != null)
            existingClassPeriod.TeacherName = updateClassPeriodDto.TeacherName;
        
        if (updateClassPeriodDto.PeriodNumber != null)
            existingClassPeriod.PeriodNumber = updateClassPeriodDto.PeriodNumber.Value;
        
        if (updateClassPeriodDto.DayOfWeek != null)
            existingClassPeriod.DayOfWeek = updateClassPeriodDto.DayOfWeek.Value;
        
        var result = unitOfWork.ScheduleRepository.UpdateClassPeriod(existingClassPeriod);
        if (!result.IsSuccess)
            return Result<ClassPeriodDto>.Failure(result.ErrorMessage!);
        
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

    public async Task<Result<ScheduleSettingDto>> GetScheduleSettingsAsync(Guid classId)
    {
        var result = await unitOfWork.ScheduleRepository.GetScheduleSettingByClassIdAsync(classId);
        if (!result.IsSuccess)
            return Result<ScheduleSettingDto>.Failure(result.ErrorMessage!);
        
        var scheduleSettingDto = mapper.Map<ScheduleSettingDto>(result.Value);
        return Result<ScheduleSettingDto>.Success(scheduleSettingDto);
    }

    public async Task<Result> UpdateScheduleSettingAsync(Guid scheduleSettingId, UpdateScheduleSettingDto updateScheduleSettingDetailDto)
    {
        var scheduleSettingResult = await unitOfWork.ScheduleRepository.GetScheduleSettingByIdAsync(scheduleSettingId);
        if (!scheduleSettingResult.IsSuccess)
            return Result.Failure(scheduleSettingResult.ErrorMessage!);
        
        var scheduleSetting = scheduleSettingResult.Value!;
        scheduleSetting.MorningPeriodCount = updateScheduleSettingDetailDto.MorningPeriodCount;
        scheduleSetting.AfternoonPeriodCount = updateScheduleSettingDetailDto.AfternoonPeriodCount;

        var periodCount = scheduleSetting.MorningPeriodCount + scheduleSetting.AfternoonPeriodCount;
        var removedSettings =
            await unitOfWork.ScheduleRepository.CleanOverflowScheduleSettingsAsync(scheduleSettingId, periodCount);
        scheduleSetting.Details.RemoveRange(removedSettings);

        foreach (var detail in updateScheduleSettingDetailDto.Details)
        {
            if (detail.PeriodNumber > periodCount)
                continue;
            
            var existingDetail = scheduleSetting.Details
                .FirstOrDefault(d => d.PeriodNumber == detail.PeriodNumber);
            if (existingDetail != null)
            {
                existingDetail.StartTime = detail.StartTime;
                existingDetail.EndTime = detail.EndTime;
            }
            else
            {
                var newDetail = new ScheduleSettingDetail
                {
                    ScheduleSettingId = scheduleSettingId,
                    PeriodNumber = detail.PeriodNumber,
                    StartTime = detail.StartTime,
                    EndTime = detail.EndTime
                };
                scheduleSetting.Details.Add(newDetail);
            }
        }
        
        var updateResult = unitOfWork.ScheduleRepository.UpdateScheduleSetting(scheduleSetting, scheduleSetting.Details);
        if (!updateResult.IsSuccess)
            return Result.Failure(updateResult.ErrorMessage!);
        
        await unitOfWork.SaveChangesAsync();
        return Result.Success();
    }
}