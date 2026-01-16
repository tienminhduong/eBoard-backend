using eBoardAPI.Common;
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

    public async Task<ClassPeriod> AddClassPeriodAsync(ClassPeriod classPeriod)
    {
        await dbContext.ClassPeriods.AddAsync(classPeriod);
        return classPeriod;
    }

    public Result<ClassPeriod> UpdateClassPeriod(ClassPeriod classPeriod)
    {
        try
        {
            dbContext.ClassPeriods.Update(classPeriod);
            return Result<ClassPeriod>.Success(classPeriod);
        }
        catch (Exception ex)
        {
            return Result<ClassPeriod>.Failure($"Cập nhật tiết học thất bại: {ex.Message}");
        }
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
        
        for (var i = 0; i < scheduleSetting.MorningPeriodCount + scheduleSetting.AfternoonPeriodCount; i++)
        {
            var detail = new ScheduleSettingDetail
            {
                ScheduleSettingId = scheduleSetting.Id,
                PeriodNumber = i + 1,
                StartTime = TimeOnly.Parse("08:00"),
                EndTime = TimeOnly.Parse("08:45")
            };
            await dbContext.ScheduleSettingDetails.AddAsync(detail);
        }
        return scheduleSetting;
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
            .Include(s => s.Details)
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