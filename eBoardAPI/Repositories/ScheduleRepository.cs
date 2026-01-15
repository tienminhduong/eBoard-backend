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
            .ToListAsync();
    }
}