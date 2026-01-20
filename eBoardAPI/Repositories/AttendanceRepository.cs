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
}