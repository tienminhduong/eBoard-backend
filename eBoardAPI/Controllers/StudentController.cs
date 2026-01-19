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
        var result = await studentService.GetByIdAsync(id);
        return result.IsSuccess ? Ok(result.Value) : NotFound(result.ErrorMessage);
    }
    
    // authorize as teacher
    [HttpPost]
    public async Task<ActionResult<StudentInfoDto>> CreateStudent([FromBody] CreateStudentDto student)
    {
        var result = await studentService.AddNewStudentAsync(student);
        return result.IsSuccess ? CreatedAtAction(nameof(GetStudentById), new { id = result.Value!.Id }, result.Value) 
                                : BadRequest(result.ErrorMessage);
    }
    
    [HttpGet("{classId}/lists")]
    public async Task<ActionResult> GetStudentsOptionInClass([FromRoute] Guid classId)
    {
        var result = await studentService.GetStudentsOptionInClassAsync(classId);
        var options = result.Select(item => new { id = item.Item1, fullName = item.Item2 });
        return Ok(options);
    }
    
    [HttpPatch("{id}")]
    public async Task<ActionResult<StudentInfoDto>> UpdateStudentInfo([FromRoute] Guid id, [FromBody] UpdateStudentInfoDto updateStudentInfoDto)
    {
        var result = await studentService.UpdateStudentInfoAsync(id, updateStudentInfoDto);
        return result.IsSuccess ? NoContent() : NotFound(result.ErrorMessage);
    }
}