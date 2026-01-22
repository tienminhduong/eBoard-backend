using eBoardAPI.Common;
using eBoardAPI.Models.AbsentRequest;
using eBoardAPI.Models.Attendance;

namespace eBoardAPI.Interfaces.Services;

public interface IAttendanceService
{
    Task<Result<AttendanceInfoByClassDto>> GetAttendanceInfoByClassAsync(Guid classId, DateOnly date);
    Task<Result<AttendanceInfoByClassDto>> CreateAttendaceForDateAsync(CreateAttendaceForDateDto dto);
    Task<Result> PatchAttendanceRecordAsync(Guid attendanceId, PatchAttendanceDto dto);
    
    Task<Result> RegisterAbsencesForStudentInClassAsync(CreateAbsentRequestDto requestDto);
    Task <Result> ApproveAbsenceRequestAsync(Guid requestId);
    Task <Result> RejectAbsenceRequestAsync(Guid requestId);
    Task<IEnumerable<AbsentRequestDto>> GetAbsentRequestsForClassAsync(Guid classId, string status, int pageNumber, int pageSize);
    Task<Result<ClassAttendanceSummary>> GetClassAttendanceSummaryAsync(Guid classId, DateOnly date);
    
    Task SendNotificationForAbsenceWithoutExcuseToParentsAsync(Guid classId, DateOnly date);
    Task<IEnumerable<string>> GetRecentPickUpPersonForStudentAsync(Guid studentId, int limit);
}