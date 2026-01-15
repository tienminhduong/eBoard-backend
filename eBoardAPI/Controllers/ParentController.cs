using eBoardAPI.Interfaces.Services;
using eBoardAPI.Models;
using eBoardAPI.Models.Parent;
using Microsoft.AspNetCore.Mvc;

namespace eBoardAPI.Controllers;

[ApiController]
[Route("api/parents")]
public class ParentController(IParentService parentService) : ControllerBase
{
    // authorize for only that parent and teachers who teach their children
    [HttpGet("info/{id}")]
    public async Task<ActionResult<ParentInfoDto>> GetParentInfo([FromRoute] string id)
    {
        var guidId = Guid.TryParse(id, out var parsedId) ? parsedId : Guid.Empty;

        try
        {
            var parentInfo = await parentService.GetByIdAsync(guidId);

            if (parentInfo is null)
            {
                return NotFound($"Parent with ID {id} not found.");
            }
            return Ok(parentInfo);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // same with above
    [HttpPut("info/{id}")]
    public async Task<ActionResult> UpdateParentInfo([FromRoute] string id, [FromBody] UpdateParentInfoDto updateParentInfoDto)
    {
        var guidId = Guid.TryParse(id, out var parsedId) ? parsedId : Guid.Empty;
        try
        {
            var updateParent = await parentService.UpdateAsync(guidId, updateParentInfoDto);
            if (updateParent is null)
            {
                return NotFound($"Parent with ID {id} not found.");
            }

            return Ok(updateParent);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}