using eBoardAPI.Consts;
using eBoardAPI.Interfaces.Services;
using eBoardAPI.Models.AbsentRequest;
using eBoardAPI.Models.Attendance;
using Microsoft.AspNetCore.Mvc;

namespace eBoardAPI.Controllers;

[ApiController]
[Route("api/attendance")]
public class AttendanceController(IAttendanceService attendanceService) : ControllerBase
{
    [HttpGet("class/{classId}/date/{date}")]
    public async Task<ActionResult<AttendanceInfoByClassDto>> GetAttendanceInfoByClassAsync(Guid classId, DateOnly date)
    {
        var attendanceInfoResult = await attendanceService.GetAttendanceInfoByClassAsync(classId, date);
        if (!attendanceInfoResult.IsSuccess)
            return NotFound(attendanceInfoResult.ErrorMessage);
        return Ok(attendanceInfoResult.Value);
    }
    
    [HttpGet("class/{classId}/date/{date}/summary")]
    public async Task<ActionResult<ClassAttendanceSummary>> GetClassAttendanceSummaryAsync(Guid classId, DateOnly date)
    {
        var summaryResult = await attendanceService.GetClassAttendanceSummaryAsync(classId, date);
        if (!summaryResult.IsSuccess)
            return NotFound(summaryResult.ErrorMessage);
        return Ok(summaryResult.Value);
    }
    
    [HttpPost]
    public async Task<ActionResult<AttendanceInfoByClassDto>> CreateAttendaceForDateAsync([FromBody] CreateAttendaceForDateDto dto)
    {
        var attendanceInfoResult = await attendanceService.CreateAttendaceForDateAsync(dto);
        if (!attendanceInfoResult.IsSuccess)
            return BadRequest(attendanceInfoResult.ErrorMessage);
        var attendanceInfo = attendanceInfoResult.Value!;
        return Ok(attendanceInfo);
    }

    [HttpPatch("{attendanceId}")]
    public async Task<ActionResult> PatchAttendanceRecordAsync(Guid attendanceId, [FromBody] PatchAttendanceDto dto)
    {
        var result = await attendanceService.PatchAttendanceRecordAsync(attendanceId, dto);
        return result.IsSuccess ? NoContent() : BadRequest(result.ErrorMessage);
    }

    [HttpPost("absent-request")]
    public async Task<ActionResult> RegisterAbsencesForStudentInClassAsync([FromBody] CreateAbsentRequestDto requestDto)
    {
        var result = await attendanceService.RegisterAbsencesForStudentInClassAsync(requestDto);
        if (!result.IsSuccess)
            return BadRequest($"Thất bại khi đăng ký đơn xin nghỉ: {result.ErrorMessage}");
        return Ok();
    }

    [HttpPost("absent-request/{requestId}/approve")]
    public async Task<ActionResult> ApproveAbsenceRequestAsync(Guid requestId)
    {
        var result = await attendanceService.ApproveAbsenceRequestAsync(requestId);
        return result.IsSuccess ? Ok() : BadRequest(result.ErrorMessage);
    }

    [HttpPost("absent-request/{requestId}/reject")]
    public async Task<ActionResult> RejectAbsenceRequestAsync(Guid requestId)
    {
        var result = await attendanceService.RejectAbsenceRequestAsync(requestId);
        return result.IsSuccess ? Ok() : BadRequest(result.ErrorMessage);
    }

    [HttpGet("absent-requests/class/{classId}/pending")]
    public async Task<ActionResult<IEnumerable<AbsentRequestDto>>> GetPendingAbsentRequestsForClassAsync(Guid classId,
        [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var absentRequests =
            await attendanceService.GetAbsentRequestsForClassAsync(classId, EAbsentRequestStatus.PENDING, pageNumber, pageSize);
        return Ok(absentRequests);
    }

    [HttpGet("absent-requests/class/{classId}/approved")]
    public async Task<ActionResult<IEnumerable<AbsentRequestDto>>> GetApprovedAbsentRequestsForClassAsync(Guid classId,
        [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var absentRequests =
            await attendanceService.GetAbsentRequestsForClassAsync(classId, EAbsentRequestStatus.APPROVED, pageNumber,
                pageSize);
        return Ok(absentRequests);
    }

    [HttpGet("absent-requests/class/{classId}/rejected")]
    public async Task<ActionResult<IEnumerable<AbsentRequestDto>>> GetRejectedAbsentRequestsForClassAsync(Guid classId,
        [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var absentRequests =
            await attendanceService.GetAbsentRequestsForClassAsync(classId, EAbsentRequestStatus.REJECTED, pageNumber,
                pageSize);
        return Ok(absentRequests);
    }

    [HttpGet("absent-requests/class/{classId}/all")]
    public async Task<ActionResult<IEnumerable<AbsentRequestDto>>> GetAllAbsentRequestsForClassAsync(Guid classId,
        [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var absentRequests =
            await attendanceService.GetAbsentRequestsForClassAsync(classId, string.Empty, pageNumber,
                pageSize);
        return Ok(absentRequests);
    }
}