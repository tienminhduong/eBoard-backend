using Microsoft.AspNetCore.Mvc;

namespace eBoardAPI.Controllers;

[ApiController]
[Route("api/classes")]
public class ClassController : ControllerBase
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
    [HttpGet("/api/classes")]
    public async Task<ActionResult> GetAllClassesByTeacher()
    {
        var classes = new[]
        {
            new
            {
                Id = Guid.NewGuid(),
                Grade = new { Id = Guid.NewGuid(), Name = "Grade 1" },
                RoomName = "Room A",
                StartDate = new DateOnly(2023, 9, 1),
                EndDate = new DateOnly(2024, 6, 15),
                MaxCapacity = 30,
                Description = "This is Grade 1 class."
            },
            new
            {
                Id = Guid.NewGuid(),
                Grade = new { Id = Guid.NewGuid(), Name = "Grade 2" },
                RoomName = "Room B",
                StartDate = new DateOnly(2023, 9, 1),
                EndDate = new DateOnly(2024, 6, 15),
                MaxCapacity = 25,
                Description = "This is Grade 2 class."
            }
        };
        return Ok(classes);
    }
}