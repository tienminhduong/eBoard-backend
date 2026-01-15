using eBoardAPI.Consts;
using eBoardAPI.Interfaces.Services;
using eBoardAPI.Models;
using eBoardAPI.Models.Parent;
using eBoardAPI.Models.Student;
using Microsoft.AspNetCore.Mvc;

namespace eBoardAPI.Controllers;

[ApiController]
[Route("api/students")]
public class StudentController(IStudentService studentService) : ControllerBase
{
    // authorize as teacher or parent
    [HttpGet("{id}")]
    public async Task<ActionResult<StudentInfoDto>> GetStudentById([FromRoute] string id)
    {
        if(string.IsNullOrWhiteSpace(id))
        {
            return BadRequest("Student ID is required.");
        }
        var guidId = Guid.Parse(id);
        if(guidId == Guid.Empty)
        {
            return BadRequest("Invalid student ID.");
        }

        try
        {
            var studentInfo = await studentService.GetByIdAsync(guidId);
            if(studentInfo == null)
            {
                return NotFound("Student not found.");
            }
            return Ok(studentInfo);
        }
        catch(Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
    
    // authorize as teacher
    [HttpPost]
    public async Task<ActionResult<StudentInfoDto>> CreateStudent([FromBody] CreateStudentDto student)
    {
        if(!ModelState.IsValid)
            return BadRequest();

        try
        {
            var studentInfo = await studentService.CreateAsync(student);
            if (studentInfo == null)
                return BadRequest();
            return CreatedAtAction(nameof(GetStudentById), new { id = studentInfo.Id }, studentInfo);
        }
        catch(Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}