using eBoardAPI.Entities;
using eBoardAPI.Interfaces.Services;
using eBoardAPI.Models.Class;
using eBoardAPI.Models.Student;
using eBoardAPI.Models.Subject;
using Microsoft.AspNetCore.Mvc;

namespace eBoardAPI.Controllers;

[ApiController]
[Route("api/classes")]
public class ClassController(IClassService classService) : ControllerBase
{
    [HttpGet("/api/grades")]
    public async Task<ActionResult<IEnumerable<Grade>>> GetAllGrades()
    {
        var grades = await classService.GetAllGradesAsync();
        return Ok(grades);
    }
    
    [HttpGet]
    public async Task<ActionResult<ClassInfoDto>> GetClassesByTeacher(
        Guid teacherId,
        int pageNumber = 1,
        int pageSize = 20 /*later replaced by id from access token*/)
    {
        var classDtos = await classService.GetPagedClassesByTeacherAsync(teacherId, pageNumber, pageSize);
        return Ok(classDtos);
    }
    
    // authorize as teacher
    [HttpGet("teaching")]
    public async Task<ActionResult<ClassInfoDto>> GetAllClassesTeachingByTeacher(Guid teacherId /*later replaced by id from access token*/)
    {
        var classDtos = await classService.GetAllTeachingClassesByTeacherAsync(teacherId);
        return Ok(classDtos);
    }
    
    [HttpGet("teaching/lists")]
    public async Task<ActionResult<ClassInfoDto>> GetAllClassesOptionTeachingByTeacher(Guid teacherId /*later replaced by id from access token*/)
    {
        var classDtos = await classService.GetAllTeachingClassesByTeacherAsync(teacherId);
        var options = classDtos.Select(c => new { id = c.Id, name = c.Name });
        return Ok(options);
    }
    
    [HttpGet("{classId}")]
    public async Task<ActionResult<ClassInfoDto>> GetClassById(Guid classId)
    {
        var result = await classService.GetClassByIdAsync(classId);
        return result.IsSuccess ? Ok(result.Value!) : NotFound(result.ErrorMessage!);
    }

    [HttpGet("{classId}/students")]
    public async Task<ActionResult<PagedStudentInClassDto>> GetStudentsByClassId(Guid classId, int pageNumber = 1, int pageSize = 20)
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

    [HttpGet("{classId}/subjects")]
    public async Task<ActionResult<IEnumerable<SubjectDto>>> GetSubjectsInClass(Guid classId)
    {
        var subjectDtos = await classService.GetSubjectInClassAsync(classId);
        return Ok(subjectDtos);
    }

    [HttpDelete("{classId}/students/{studentId}")]
    public async Task<ActionResult> RemoveStudentFromClass(Guid classId, Guid studentId)
    {
        var result = await classService.RemoveStudentFromClassAsync(classId, studentId);
        return result.IsSuccess ? NoContent() : BadRequest(result.ErrorMessage!);
    }
}