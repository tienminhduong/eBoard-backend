using eBoardAPI.Consts;
using eBoardAPI.Interfaces.Services;
using eBoardAPI.Models.Activity;
using Microsoft.AspNetCore.Mvc;

namespace eBoardAPI.Controllers;

[ApiController]
[Route("api/activities")]
public class ActivitySignInController(IActivityService activityService) : ControllerBase
{
    [HttpPost("signins")]
    public async Task<ActionResult> AddSignIn([FromBody] AddActivitySignInDto addSignInDto)
    {
        var result = await activityService.AddSignInAsync(addSignInDto);
        if (!result.IsSuccess)
        {
            return BadRequest(result.ErrorMessage!);
        }
        var res = await activityService.CheckPaidSignInAsync(result.Value);
        return res.IsSuccess ? NoContent() : BadRequest(res.ErrorMessage);
    }

    [HttpDelete("signins/{id}")]
    public async Task<ActionResult> RemoveSignIn(Guid id)
    {
        var result = await activityService.RemoveSignInAsync(id);
        return result.IsSuccess ? NoContent() : BadRequest(result.ErrorMessage!);
    }

    [HttpPost("signins/{id}/accept")]
    public async Task<ActionResult> AcceptSignIn(Guid id)
    {
        var result = await activityService.AcceptSignInAsync(id);
        return result.IsSuccess ? NoContent() : BadRequest(result.ErrorMessage!);
    }

    [HttpPost("signins/{id}/reject")]
    public async Task<ActionResult> RejectSignIn(Guid id)
    {
        var result = await activityService.RejectSignInAsync(id);
        return result.IsSuccess ? NoContent() : BadRequest(result.ErrorMessage!);
    }

    [HttpPost("signins/{id}/checkpaid")]
    public async Task<ActionResult> CheckPaidSignIn(Guid id)
    {
        var result = await activityService.CheckPaidSignInAsync(id);
        return result.IsSuccess ? NoContent() : BadRequest(result.ErrorMessage!);
    }

    [HttpGet("{classId}/signins/pending")]
    public async Task<ActionResult> GetSignInsForActivity(Guid classId)
    {
        var signIns = await activityService.GetSignInsForActivityAsync(classId, EActivitySignInStatus.PENDING);
        return Ok(signIns);
    }

    [HttpGet("{classId}/signins/accepted")]
    public async Task<ActionResult> GetSignInsForActivityByStatus(Guid classId)
    {
        var signIns = await activityService.GetSignInsForActivityAsync(classId, EActivitySignInStatus.ACCEPTED);
        return Ok(signIns);
    }
    
    [HttpGet("{classId}/signins/rejected")]
    public async Task<ActionResult> GetRejectedSignInsForActivity(Guid classId)
    {
        var signIns = await activityService.GetSignInsForActivityAsync(classId, EActivitySignInStatus.REJECTED);
        return Ok(signIns);
    }
    
    [HttpGet("{classId}/signins/paid")]
    public async Task<ActionResult> GetPaidSignInsForActivity(Guid classId)
    {
        var signIns = await activityService.GetSignInsForActivityAsync(classId, EActivitySignInStatus.PAID);
        return Ok(signIns);
    }

    [HttpGet("{classId}/signins")]
    public async Task<ActionResult> GetAllSignInsForActivity(Guid classId)
    {
        var signIns = await activityService.GetSignInsForActivityAsync(classId, string.Empty);
        return Ok(signIns);
    }
}