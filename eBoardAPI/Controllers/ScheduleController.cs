using eBoardAPI.Interfaces.Services;
using eBoardAPI.Models.Schedule;
using Microsoft.AspNetCore.Mvc;

namespace eBoardAPI.Controllers;

[ApiController]
[Route("api/schedule")]
public class ScheduleController(IScheduleService scheduleService) : ControllerBase
{
    [HttpGet("{classId}")]
    public async Task<ActionResult<ScheduleDto>> GetScheduleForClass(Guid classId)
    {
        var scheduleResult = await scheduleService.GetClassPeriodsByClassAsync(classId);
        return scheduleResult.IsSuccess ? Ok(scheduleResult.Value) : BadRequest(scheduleResult.ErrorMessage);
    }

    [HttpPatch("periods")]
    public async Task<IActionResult> UpdateClassPeriods(Guid classPeriodId, [FromBody] UpdateClassPeriodDto periodDto)
    {
        var updateResult = await scheduleService.UpdateClassPeriodAsync(classPeriodId, periodDto);
        return updateResult.IsSuccess ? NoContent() : BadRequest(updateResult.ErrorMessage);
    }
    
    [HttpPost("periods")]
    public async Task<IActionResult> AddClassPeriod([FromBody] CreateClassPeriodDto periodDto)
    {
        var result = await scheduleService.AddClassPeriodAsync(periodDto);
        if (!result.IsSuccess)
            return BadRequest(result.ErrorMessage);
        return Ok(new
        {
            message = "Created class period successfully",
            data = result.Value,
            createdAt = DateTime.UtcNow
        });
    }

    [HttpDelete("periods/{classPeriodId}")]
    public async Task<IActionResult> DeleteClassPeriod(Guid classPeriodId)
    {
        var result = await scheduleService.DeleteClassPeriodAsync(classPeriodId);
        return result ? NoContent() : BadRequest("Không tìm thấy tiết học để xóa");
    }
    
    [HttpGet("{classId}/settings")]
    public async Task<ActionResult<ScheduleSettingDto>> GetScheduleSettings(Guid classId)
    {
        var result = await scheduleService.GetScheduleSettingsAsync(classId);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.ErrorMessage);
    }

    [HttpPut("settings/{scheduleSettingId}")]
    public async Task<IActionResult> UpdateScheduleSetting(Guid scheduleSettingId,
        [FromBody] UpdateScheduleSettingDto settingDto)
    {
        var result = await scheduleService.UpdateScheduleSettingAsync(scheduleSettingId, settingDto);
        return result.IsSuccess ? NoContent() : BadRequest(result.ErrorMessage);
    }
}