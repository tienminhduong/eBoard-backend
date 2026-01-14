using eBoardAPI.Models;
using eBoardAPI.Models.Teacher;
using Microsoft.AspNetCore.Mvc;

namespace eBoardAPI.Controllers;

[ApiController]
[Route("api/teachers")]
public class TeacherController : ControllerBase
{
    // authorize parent and teacher role
    [HttpGet("info/{id}")]
    public async Task<ActionResult<TeacherInfoDto>> GetTeacherInfo([FromRoute] int id)
    {
        return Ok(new TeacherInfoDto
        {
            Id = Guid.NewGuid(),
            FullName = "Tien Minh Duong",
            Email = "tien.minh.duong@example.com",
            PhoneNumber = "123-456-7890",
            Qualifications = "MSc in Mathematics"
        });
    }

    // authorize teacher role
    [HttpPut("info/{id}")]
    public async Task<ActionResult> UpdateTeacherInfo([FromRoute] int id, [FromBody] UpdateTeacherInfoDto updateTeacherInfoDto)
    {
        return Ok();
    }
}
