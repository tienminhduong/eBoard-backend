using eBoardAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace eBoardAPI.Controllers;

[ApiController]
[Route("api/students")]
public class StudentController : ControllerBase
{
    // authorize as teacher or parent
    [HttpGet("{id}")]
    public async Task<ActionResult<StudentInfoDto>> GetStudentById(int id)
    {
        var student = new StudentInfoDto
        {
            Id = id,
            FirstName = "John",
            LastName = "Doe",
            FullAddress = "123 Main St, Springfield",
            RelationshipWithParent = RelationshipWithParent.Father,
            DateOfBirth = new DateOnly(2010, 5, 15),
            Gender = Gender.Male,
            Parent = new ParentInfoDto
            {
                Id = Guid.NewGuid(),
                FullName = "Michael Doe",
                PhoneNumber = "123-456-7890",
                Email = "lmao@example.com",
                HealthCondition = "Healthy",
                Address = "123 Main St, Springfield"
            }
        };

        return Ok(student);
    }
}