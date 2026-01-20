using eBoardAPI.Common;
using eBoardAPI.Models.Attendance;

namespace eBoardAPI.Interfaces.Services;

public interface IAttendanceService
{
    Task<Result<AttendanceInfoByClassDto>> GetAttendanceInfoByClassAsync(Guid classId, DateOnly date);
    Task<Result<AttendanceInfoByClassDto>> CreateAttendaceForDateAsync(CreateAttendaceForDateDto dto);
    Task<Result> PatchAttendanceRecordAsync(Guid attendanceId, PatchAttendanceDto dto);
}