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
    
}