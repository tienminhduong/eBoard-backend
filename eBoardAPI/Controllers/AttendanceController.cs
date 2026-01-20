using eBoardAPI.Interfaces.Services;
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
}