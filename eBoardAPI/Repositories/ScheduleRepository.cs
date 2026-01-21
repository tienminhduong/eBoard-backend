using eBoardAPI.Common;
using eBoardAPI.Consts;
using eBoardAPI.Context;
using eBoardAPI.Entities;
using eBoardAPI.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace eBoardAPI.Repositories;

public class ScheduleRepository(AppDbContext dbContext) : IScheduleRepository
{
    public async Task<Result<ClassPeriod>> GetClassPeriodByIdAsync(Guid classPeriodId)
    {
        var classPeriod = await dbContext.ClassPeriods.FindAsync(classPeriodId);
        return classPeriod == null
            ? Result<ClassPeriod>.Failure("Không tìm thấy tiết học")
            : Result<ClassPeriod>.Success(classPeriod);
    }

    public async Task<Result<ClassPeriod>> AddClassPeriodAsync(ClassPeriod classPeriod)
    {
        var query = from setting in dbContext.ScheduleSettings
                    where setting.ClassId == classPeriod.ClassId
                    select setting;
        var scheduleSetting = await query.FirstOrDefaultAsync();
        if (scheduleSetting == null)
            return Result<ClassPeriod>.Failure("Không tìm thấy cài đặt thời khóa biểu cho lớp học này");
        
        if (classPeriod.IsMorningPeriod && classPeriod.PeriodNumber > scheduleSetting.MorningPeriodCount ||
            !classPeriod.IsMorningPeriod && classPeriod.PeriodNumber > scheduleSetting.AfternoonPeriodCount)
        {
            return Result<ClassPeriod>.Failure("Tiết học vượt quá số tiết học trong cài đặt thời khóa biểu");
        }
        
        await dbContext.ClassPeriods.AddAsync(classPeriod);
        return Result<ClassPeriod>.Success(classPeriod);
    }

    public async Task<Result<ClassPeriod>> UpdateClassPeriod(ClassPeriod classPeriod)
    {
        var query = from setting in dbContext.ScheduleSettings
            where setting.ClassId == classPeriod.ClassId
            select setting;
        var scheduleSetting = await query.FirstOrDefaultAsync();
        if (scheduleSetting == null)
            return Result<ClassPeriod>.Failure("Không tìm thấy cài đặt thời khóa biểu cho lớp học này");
        
        if (classPeriod.IsMorningPeriod && classPeriod.PeriodNumber > scheduleSetting.MorningPeriodCount ||
            !classPeriod.IsMorningPeriod && classPeriod.PeriodNumber > scheduleSetting.AfternoonPeriodCount)
        {
            return Result<ClassPeriod>.Failure("Tiết học vượt quá số tiết học trong cài đặt thời khóa biểu");
        }
        
        dbContext.ClassPeriods.Update(classPeriod);
        return Result<ClassPeriod>.Success(classPeriod);
    }

    public async Task<bool> DeleteClassPeriodAsync(Guid classPeriodId)
    {
        var classPeriod = await dbContext.ClassPeriods.FindAsync(classPeriodId);
        if (classPeriod == null)
            return false;

        dbContext.ClassPeriods.Remove(classPeriod);
        await dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<ClassPeriod>> GetClassPeriodsByClassAsync(Guid classId)
    {
        var query = from cp in dbContext.ClassPeriods
                    where cp.ClassId == classId
                    orderby cp.DayOfWeek, cp.PeriodNumber
                    select cp;

        return await query
            .AsNoTracking()
            .Include(cp => cp.Subject)
            .ToListAsync();
    }

    public async Task<ScheduleSetting> AddNewScheduleSettingAsync(ScheduleSetting scheduleSetting)
    {
        await dbContext.ScheduleSettings.AddAsync(scheduleSetting);
        await GenerateNewScheduleSettingDetailsAsync(scheduleSetting);
        
        return scheduleSetting;
    }

    private async Task GenerateNewScheduleSettingDetailsAsync(ScheduleSetting scheduleSetting)
    {
        var startTime = ClassSettingConst.MorningStateTime;
        for (var i = 0; i < scheduleSetting.MorningPeriodCount; i++)
        {
            var detail = new ScheduleSettingDetail
            {
                ScheduleSettingId = scheduleSetting.Id,
                PeriodNumber = i + 1,
                StartTime = startTime,
                EndTime = startTime.AddMinutes(ClassSettingConst.PERIOD_DURATION_MINUTES),
                IsMorningPeriod = true
            };
            await dbContext.ScheduleSettingDetails.AddAsync(detail);
            startTime = detail.EndTime.AddMinutes(ClassSettingConst.BREAK_DURATION_MINUTES);
        }
        
        startTime = ClassSettingConst.AfternoonStartTime;
        for (var i = 0; i < scheduleSetting.AfternoonPeriodCount; i++)
        {
            var detail = new ScheduleSettingDetail
            {
                ScheduleSettingId = scheduleSetting.Id,
                PeriodNumber = i + 1,
                StartTime = startTime,
                EndTime = startTime.AddMinutes(ClassSettingConst.PERIOD_DURATION_MINUTES),
                IsMorningPeriod = false
            };
            await dbContext.ScheduleSettingDetails.AddAsync(detail);
            startTime = detail.EndTime.AddMinutes(ClassSettingConst.BREAK_DURATION_MINUTES);
        }
    }

    public Result<ScheduleSetting> UpdateScheduleSetting(ScheduleSetting scheduleSetting, IEnumerable<ScheduleSettingDetail> details)
    {
        try
        {
            dbContext.ScheduleSettings.Update(scheduleSetting);
            dbContext.ScheduleSettingDetails.UpdateRange(details);
            return Result<ScheduleSetting>.Success(scheduleSetting);
        }
        catch (Exception ex)
        {
            return Result<ScheduleSetting>.Failure($"Cập nhật cài đặt thời khóa biểu thất bại: {ex.Message}");
        }
    }

    public async Task<Result<ScheduleSetting>> GetScheduleSettingByClassIdAsync(Guid classId)
    {
        var query = from ss in dbContext.ScheduleSettings
                    where ss.ClassId == classId
                    select ss;

        var scheduleSetting = await query
            .AsNoTracking()
            .Include(s => s.Details.OrderBy(d => d.StartTime))
            .FirstOrDefaultAsync();

        return scheduleSetting == null
            ? Result<ScheduleSetting>.Failure("Không tìm thấy cài đặt thời khóa biểu")
            : Result<ScheduleSetting>.Success(scheduleSetting);
    }

    public async Task<Result<ScheduleSetting>> GetScheduleSettingByIdAsync(Guid scheduleSettingId)
    {
        var scheduleSetting = await dbContext.ScheduleSettings
            .Include(s => s.Details)
            .FirstOrDefaultAsync(s => s.Id == scheduleSettingId);

        return scheduleSetting == null
            ? Result<ScheduleSetting>.Failure("Không tìm thấy cài đặt thời khóa biểu")
            : Result<ScheduleSetting>.Success(scheduleSetting);
    }

    public async Task<IEnumerable<ScheduleSettingDetail>> CleanUpOverflowScheduleSettingsAsync(Guid scheduleSettingId,
        int validMorningPeriodCount, int validAfternoonPeriodCount)
    {
        var query = from d in dbContext.ScheduleSettingDetails
                    where d.ScheduleSettingId == scheduleSettingId &&
                          (d.IsMorningPeriod && d.PeriodNumber > validMorningPeriodCount
                            || !d.IsMorningPeriod && d.PeriodNumber > validAfternoonPeriodCount)
                    select d;
        
        var overflowDetails = await query.ToListAsync();
        
        dbContext.ScheduleSettingDetails.RemoveRange(overflowDetails);
        return overflowDetails;
    }

    public async Task AddNewScheduleSettingDetailAsync(ScheduleSettingDetail scheduleSettingDetails)
    {
        await dbContext.ScheduleSettingDetails.AddAsync(scheduleSettingDetails);
    }

    public async Task<bool> ValidateEditableScheduleSettingAsync(Guid classId, int morningPeriodCount, int afternoonPeriodCount)
    {
        var query = from cp in dbContext.ClassPeriods
                    where cp.ClassId == classId &&
                          ((cp.PeriodNumber > morningPeriodCount && cp.IsMorningPeriod) ||
                           (cp.PeriodNumber > afternoonPeriodCount && !cp.IsMorningPeriod))
                    select cp;
        
        return !await query.AsNoTracking().AnyAsync();
    }
}