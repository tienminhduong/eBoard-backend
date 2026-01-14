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
        var grades = new[]
        {
            new { Id = Guid.NewGuid(), Name = "Grade 1" },
            new { Id = Guid.NewGuid(), Name = "Grade 2" },
            new { Id = Guid.NewGuid(), Name = "Grade 3" },
        };
        return Ok(grades);
    }
    
    // authorize as teacher
    [HttpGet]
    public async Task<ActionResult> GetAllClassesByTeacher(Guid teacherId)
    {
        var classDtos = await classService.GetAllTeachingClassesByTeacher(teacherId);
        return Ok(classDtos);
    }
    
    [HttpGet("{classId}")]
    public async Task<ActionResult> GetClassById(Guid classId)
    {
        var classInfo = new ClassInfoDto
        {
            Grade = "Grade 1",
            Name = "Grade 1",
            TeacherName =  "Teacher A",
            RoomName = "Room A",
            MaxCapacity = 30,
            Description = "This is Grade 1 class."
        };
        return Ok(classInfo);
    }

    [HttpGet("{classId}/students")]
    public async Task<ActionResult> GetStudentsByClassId(Guid classId)
    {
        return Ok();
    }
    
    [HttpPost]
    public async Task<ActionResult> CreateClass([FromBody] CreateClassDto createClassDto)
    {
        return Ok();
    }
}