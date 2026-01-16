using eBoardAPI.Common;
using eBoardAPI.Entities;

namespace eBoardAPI.Interfaces.Repositories;

public interface IScheduleRepository
{
    Task<Result<ClassPeriod>> GetClassPeriodByIdAsync(Guid classPeriodId);
    Task<ClassPeriod> AddClassPeriodAsync(ClassPeriod classPeriod);
    Result<ClassPeriod> UpdateClassPeriod(ClassPeriod classPeriod);
    Task<bool> DeleteClassPeriodAsync(Guid classPeriodId);
    Task<IEnumerable<ClassPeriod>> GetClassPeriodsByClassAsync(Guid classId);
    
    Task<ScheduleSetting> AddNewScheduleSettingAsync(ScheduleSetting scheduleSetting);
    Result<ScheduleSetting> UpdateScheduleSetting(ScheduleSetting scheduleSetting, IEnumerable<ScheduleSettingDetail> details);
    Task<Result<ScheduleSetting>> GetScheduleSettingByClassIdAsync(Guid classId);
    Task<Result<ScheduleSetting>> GetScheduleSettingByIdAsync(Guid scheduleSettingId);
    Task<IEnumerable<ScheduleSettingDetail>> CleanUpOverflowScheduleSettingsAsync(Guid scheduleSettingId,
        int validMorningPeriodCount, int validAfternoonPeriodCount);
    Task<bool> ValidateEditableScheduleSettingAsync(Guid classId, int morningPeriodCount, int afternoonPeriodCount);
}