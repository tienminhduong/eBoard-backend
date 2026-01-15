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
}