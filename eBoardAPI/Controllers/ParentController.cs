using eBoardAPI.Models;
using eBoardAPI.Models.Parent;
using Microsoft.AspNetCore.Mvc;

namespace eBoardAPI.Controllers;

[ApiController]
[Route("api/parents")]
public class ParentController : ControllerBase
{
    // authorize for only that parent and teachers who teach their children
    [HttpGet("info/{id}")]
    public async Task<ActionResult<ParentInfoDto>> GetParentInfo([FromRoute] int id)
    {
        return Ok(new ParentInfoDto
        {
            Id = Guid.NewGuid(),
            FullName = "John Doe",
            Email = "lmao.abc@example.com",
            PhoneNumber = "987-654-3210",
            Address = "123 Main St, Anytown, USA",
            HealthCondition = "Good"
        });
    }

    // same with above
    [HttpPut("info/{id}")]
    public async Task<ActionResult> UpdateParentInfo([FromRoute] int id, [FromBody] UpdateParentInfoDto updateParentInfoDto)
    {
        return Ok();

    }
}