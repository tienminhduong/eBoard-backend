using eBoardAPI.Common;
using eBoardAPI.Models.Schedule;

namespace eBoardAPI.Interfaces.Services;

public interface IScheduleService
{
    Task<Result<ClassPeriodDto>> AddClassPeriodAsync(CreateClassPeriodDto createClassPeriodDto);
    Task<Result<ClassPeriodDto>> UpdateClassPeriodAsync(Guid classPeriodId, UpdateClassPeriodDto updateClassPeriodDto);
    Task<bool> DeleteClassPeriodAsync(Guid classPeriodId);
    Task<Result<ScheduleDto>> GetClassPeriodsByClassAsync(Guid classId);
}