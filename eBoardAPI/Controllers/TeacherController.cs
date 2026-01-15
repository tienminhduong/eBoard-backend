using eBoardAPI.Interfaces.Services;
using eBoardAPI.Models;
using eBoardAPI.Models.Teacher;
using Microsoft.AspNetCore.Mvc;

namespace eBoardAPI.Controllers;

[ApiController]
[Route("api/teachers")]
public class TeacherController(ITeacherService teacherService) : ControllerBase
{
    // authorize parent and teacher role
    [HttpGet("info/{id}")]
    public async Task<ActionResult<TeacherInfoDto>> GetTeacherInfo([FromRoute] string id)
    {
        var teacherId = Guid.Parse(id);
        if(teacherId == Guid.Empty)
        {
            return BadRequest("Invalid teacher ID.");
        }
        try
        {
            var teacherInfo = await teacherService.GetTeacherInfoAsync(teacherId);
            if(teacherInfo == null)
            {
                return NotFound("Teacher not found.");
            }
            return Ok(teacherInfo);
        }
        catch(Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    // authorize teacher role
    [HttpPut("info/{id}")]
    public async Task<ActionResult> UpdateTeacherInfo([FromRoute] string id, [FromBody] UpdateTeacherInfoDto updateTeacherInfoDto)
    {
        var teacherId = Guid.Parse(id);
        if(teacherId == Guid.Empty)
        {
            return BadRequest("Invalid teacher ID.");
        }

        try
        {
            var updateInfo = await teacherService.UpdateTeacherInfoAsync(teacherId, updateTeacherInfoDto);
            if(updateInfo == null)
            {
                return NotFound("Teacher not found.");
            }
            return Ok(updateInfo);
        }
        catch(Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}
