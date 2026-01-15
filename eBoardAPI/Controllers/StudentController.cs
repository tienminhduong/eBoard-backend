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
    public async Task<ActionResult<StudentInfoDto>> GetStudentById([FromRoute] Guid id)
    {
        if (ModelState.IsValid == false)
            return BadRequest();
        
        var result = await studentService.GetByIdAsync(id);
        return result.IsSuccess ? Ok(result.Value) : NotFound(result.ErrorMessage);
    }
    
    // authorize as teacher
    [HttpPost]
    public async Task<ActionResult<StudentInfoDto>> CreateStudent([FromBody] CreateStudentDto student)
    {
        if(!ModelState.IsValid)
            return BadRequest();

        var result = await studentService.AddNewStudentAsync(student);
        return result.IsSuccess ? CreatedAtAction(nameof(GetStudentById), new { id = result.Value!.Id }, result.Value) 
                                : BadRequest(result.ErrorMessage);
    }
}