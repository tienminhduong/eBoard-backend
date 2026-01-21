using eBoardAPI.Entities;

namespace eBoardAPI.Interfaces.Repositories;

public interface IAttendanceRepository
{
    Task<IEnumerable<Attendance>> GetAttendancesByClassAndDateAsync(Guid classId, DateOnly date);
    Task<bool> ValidateAttendancesExistAsync(Guid classId, DateOnly date);
    Task CreateAttendancesAsync(IEnumerable<Attendance> attendances);
    Task<Attendance?> GetAttendanceByIdAsync(Guid attendanceId);
    void UpdateAttendanceAsync(Attendance attendance);
    Task<Tuple<int, int>> GetAttendanceCountsByClassAndDateAsync(Guid classId, DateOnly date);
}