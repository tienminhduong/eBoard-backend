using eBoardAPI.Interfaces.Services;
using eBoardAPI.Models.Class;
using Microsoft.AspNetCore.Mvc;

namespace eBoardAPI.Controllers;

[ApiController]
[Route("api/classes")]
public class ClassController(IClassService classService) : ControllerBase
{
    [HttpGet("/api/grades")]
    public async Task<ActionResult> GetAllGrades()
    {
        var grades = await classService.GetAllGradesAsync();
        return Ok(grades);
    }
    
    // authorize as teacher
    [HttpGet]
    public async Task<ActionResult> GetAllClassesByTeacher(Guid teacherId /*later replaced by id from access token*/)
    {
        var classDtos = await classService.GetAllTeachingClassesByTeacherAsync(teacherId);
        return Ok(classDtos);
    }
    
    [HttpGet("{classId}")]
    public async Task<ActionResult> GetClassById(Guid classId)
    {
        var result = await classService.GetClassByIdAsync(classId);
        return result.IsSuccess ? Ok(result.Value!) : NotFound(result.ErrorMessage!);
    }

    [HttpGet("{classId}/students")]
    public async Task<ActionResult> GetStudentsByClassId(Guid classId, int pageNumber = 1, int pageSize = 20)
    {
        var pagedStudents = await classService.GetPagedStudentsByClassAsync(classId, pageNumber, pageSize);
        return Ok(pagedStudents);
    }

    [HttpPost]
    public async Task<ActionResult> CreateClass([FromBody] CreateClassDto createClassDto,
        Guid teacherId /*later replaced by id from access token*/)
    {
        var result = await classService.AddNewClassAsync(createClassDto, teacherId);
        return result.IsSuccess
            ? CreatedAtAction(nameof(GetClassById), new { classId = result.Value!.Id }, null)
            : BadRequest(result.ErrorMessage!);
    }
}