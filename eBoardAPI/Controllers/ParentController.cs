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
        var result = await parentService.GetByIdAsync(id);
        return result.IsSuccess ? Ok(result.Value) : NotFound(result.ErrorMessage);
    }

    // same with above
    [HttpPut("info/{id}")]
    public async Task<ActionResult> UpdateParentInfo([FromRoute] Guid id, [FromBody] UpdateParentInfoDto updateParentInfoDto)
    {
        var result = await parentService.UpdateAsync(id, updateParentInfoDto);
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
    public async Task<ActionResult> GetParentNotCreateAccountByClassId(Guid classId, int pageNumber = 1, int pageSize = 20)
    {
        var result = await parentService.GetParentNotCreateAccountByClassId(classId, pageNumber, pageSize);
        return Ok(result);
    }

    [HttpGet("class/{classId}/accounts/created")]
    public async Task<ActionResult> GetParentCreateAccountByClassId(Guid classId, int pageNumber = 1, int pageSize = 20)
    {
        var result = await parentService.GetParentCreateAccountByClassId(classId, pageNumber, pageSize);
        return Ok(result);
    }

}