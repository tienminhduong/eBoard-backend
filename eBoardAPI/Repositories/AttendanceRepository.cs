using System.Collections;
using eBoardAPI.Consts;
using eBoardAPI.Context;
using eBoardAPI.Entities;
using eBoardAPI.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace eBoardAPI.Repositories;

public class AttendanceRepository(AppDbContext dbContext) : IAttendanceRepository
{
    public async Task<IEnumerable<Attendance>> GetAttendancesByClassAndDateAsync(Guid classId, DateOnly date)
    {
        var query = from attendance in dbContext.Attendances
                    where attendance.ClassId == classId && attendance.Date == date
                    orderby attendance.Status
                    select attendance;
        
        return await query
            .Include(a => a.Student)
            .Include(a => a.Class)
            .ToListAsync();
    }

    public async Task<bool> ValidateAttendancesExistAsync(Guid classId, DateOnly date)
    {
        return await dbContext.Attendances.AnyAsync(a => a.ClassId == classId && a.Date == date);
    }

    public async Task CreateAttendancesAsync(IEnumerable<Attendance> attendances)
    {
        await dbContext.Attendances.AddRangeAsync(attendances);
    }

    public async Task<Attendance?> GetAttendanceByIdAsync(Guid attendanceId)
    {
        return await dbContext.Attendances.FindAsync(attendanceId);
    }

    public void UpdateAttendanceAsync(Attendance attendance)
    {
        dbContext.Attendances.Update(attendance);
    }

    public async Task<Tuple<int, int>> GetAttendanceCountsByClassAndDateAsync(Guid classId, DateOnly date)
    {
        var query = from a in dbContext.Attendances
                    where a.ClassId == classId && a.Date == date
                    group a by 1 into g
                    select new
                    {
                        AbsentWithExcuseCount = g.Count(a => a.Status == EAttendanceStatus.EXCUSED),
                        AbsentWithoutExcuseCount = g.Count(a => a.Status == EAttendanceStatus.ABSENT)
                    };
        
        var result = await query.FirstOrDefaultAsync();
        return result != null 
            ? Tuple.Create(result.AbsentWithExcuseCount, result.AbsentWithoutExcuseCount) 
            : Tuple.Create(0, 0);
    }

    public async Task<IEnumerable<Student>> GetStudentsWithAbsenceWithoutExcuseAsync(Guid classId, DateOnly date)
    {
        var query = from attendance in dbContext.Attendances
            where attendance.ClassId == classId
                  && attendance.Status == EAttendanceStatus.ABSENT
                  && attendance.Date == date
            select attendance.Student;
        return await query.Distinct().ToListAsync();
    }
}