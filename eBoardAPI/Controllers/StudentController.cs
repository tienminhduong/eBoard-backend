using eBoardAPI.Consts;
using eBoardAPI.Models;
using eBoardAPI.Models.Parent;
using eBoardAPI.Models.Student;
using Microsoft.AspNetCore.Mvc;

namespace eBoardAPI.Controllers;

[ApiController]
[Route("api/students")]
public class StudentController : ControllerBase
{
    // authorize as teacher or parent
    [HttpGet("{id}")]
    public async Task<ActionResult<StudentInfoDto>> GetStudentById(Guid id)
    {
        var student = new StudentInfoDto
        {
            Id = id,
            FirstName = "John",
            LastName = "Doe",
            FullAddress = "123 Main St, Springfield",
            RelationshipWithParent = RelationshipWithParent.FATHER,
            DateOfBirth = new DateOnly(2010, 5, 15),
            Gender = Gender.MALE,
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
    
    // authorize as teacher
    [HttpPost]
    public async Task<ActionResult<StudentInfoDto>> CreateStudent([FromBody] CreateStudentDto student)
    {
        var createdStudent = new StudentInfoDto
        {
            Id = Guid.NewGuid(),
            FirstName = student.FirstName,
            LastName = student.LastName,
            FullAddress = student.Address,
            RelationshipWithParent = student.RelationshipWithParent,
            DateOfBirth = student.DateOfBirth,
            Gender = Gender.MALE,
        };
        return CreatedAtAction(nameof(GetStudentById), new { id = createdStudent.Id }, createdStudent);
    }
}