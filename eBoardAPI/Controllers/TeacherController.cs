using eBoardAPI.Common;
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
    public async Task<ActionResult<TeacherInfoDto>> GetTeacherInfo([FromRoute] Guid id)
    {
        if (ModelState.IsValid == false)
        {
            return BadRequest(ModelState);
        }

        var result = await teacherService.GetTeacherInfoAsync(id);
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }
        else
        {
            return NotFound(result.ErrorMessage);
        }
    }

    // authorize teacher role
    [HttpPut("info/{id}")]
    public async Task<ActionResult> UpdateTeacherInfo([FromRoute] Guid id, [FromBody] UpdateTeacherInfoDto updateTeacherInfoDto)
    {
        if(ModelState.IsValid == false)
        {
            return BadRequest(ModelState);
        }

        var result = await teacherService.UpdateTeacherInfoAsync(id, updateTeacherInfoDto);
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }
        else
        {
            return NotFound(result.ErrorMessage);
        }
    }
}
