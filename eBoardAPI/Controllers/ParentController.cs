using eBoardAPI.Interfaces.Services;
using eBoardAPI.Models;
using eBoardAPI.Models.Parent;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace eBoardAPI.Controllers;

[ApiController]
[Route("api/parents")]
public class ParentController(IParentService parentService) : ControllerBase
{
    // authorize for only that parent and teachers who teach their children
    [HttpGet("info/{id}")]
    public async Task<ActionResult<ParentInfoDto>> GetParentInfo([FromRoute] Guid id)
    {
        if(ModelState.IsValid == false)
            return BadRequest();
        var result = await parentService.GetByIdAsync(id);
        return result.IsSuccess ? Ok(result.Value) : NotFound(result.ErrorMessage);
    }

    // same with above
    [HttpPut("info/{id}")]
    public async Task<ActionResult> UpdateParentInfo([FromRoute] Guid id, [FromBody] UpdateParentInfoDto updateParentInfoDto)
    {
        if (ModelState.IsValid == false)
        {
            return BadRequest(ModelState);
        }
        var result = await parentService.GetByIdAsync(id);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.ErrorMessage);
    }

    [HttpPost("create-accounts")]
    public async Task<ActionResult> CreateAccountParents([FromBody] List<Guid> parentIds)
    {
        if (ModelState.IsValid == false)
        {
            return BadRequest(ModelState);
        }
        var result = await parentService.CreateAccountForParent(parentIds);
        return Ok(result);
    }

    [HttpGet("class/{classId}/accounts/not-created")]
    public async Task<ActionResult> GetParentNotCreateAccountByClassId(Guid classId)
    {
        var result = await parentService.GetParentNotCreateAccountByClassId(classId);
        return Ok(result);
    }

}