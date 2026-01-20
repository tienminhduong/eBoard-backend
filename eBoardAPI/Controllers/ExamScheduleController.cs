using eBoardAPI.Common;
using eBoardAPI.Interfaces.Services;
using eBoardAPI.Models.ExamSchedule;
using Microsoft.AspNetCore.Mvc;

namespace eBoardAPI.Controllers
{
    [ApiController]
    [Route("api")]
    public class ExamScheduleController(IExamScheduleService examScheduleService) : ControllerBase
    {
        [HttpPost("exams-schedule")]
        public async Task<ActionResult> CreateNewExamSchedule([FromBody] CreateExamScheduleDto createExamScheduleDto)
        {
            var result = await examScheduleService.CreateNewExamSchedule(createExamScheduleDto);
            if (result.IsSuccess)
            {
                return CreatedAtAction(nameof(GetExamScheduleById), new { examScheduleId = result.Value!.Id }, result.Value);
            }
            return BadRequest(result.ErrorMessage);
        }

        [HttpGet("exams-schedule/{examScheduleId}")]
        public async Task<ActionResult> GetExamScheduleById(Guid examScheduleId)
        {
            var result = await examScheduleService.GetExamScheduleById(examScheduleId);
            if (result.IsSuccess)
            {
                if(result.Value == null)
                {
                    return NotFound();
                }
                return Ok(result.Value);
            }
            return BadRequest(result.ErrorMessage);
        }

        [HttpGet("exams-schedule/classes/{classId}")]
        public async Task<ActionResult> GetExamSchedules(Guid classId, [FromQuery] ExamScheduleFilter filter)
        {
            var result = await examScheduleService.GetExamSchedules(classId, filter);
            if (!result.IsSuccess) 
            { 
                return BadRequest(result.ErrorMessage);
            }
            return Ok(result);
        }

        [HttpPut("exams-schedule")]
        public async Task<ActionResult> UpdateExamSchedule([FromBody] UpdateExamScheduleDto updateExamScheduleDto)
        {
            var result = await examScheduleService.UpdateExamSchedule(updateExamScheduleDto);
            if (result.IsSuccess)
            {
                return Ok();
            }
            return BadRequest(result.ErrorMessage);
        }

        [HttpDelete("exams-schedule/{examId}")]
        public async Task<ActionResult> DeleteExamSchedule(Guid examId)
        {
            var result = await examScheduleService.DeleteExamSchedule(examId);
            if (result.IsSuccess)
            {
                return Ok();
            }
            return BadRequest(result.ErrorMessage);
        }
    }
}