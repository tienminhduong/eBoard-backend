using eBoardAPI.Consts;
using eBoardAPI.Interfaces.Services;
using eBoardAPI.Models.Activity;
using Microsoft.AspNetCore.Mvc;

namespace eBoardAPI.Controllers;

[ApiController]
[Route("api/activities")]
public class ActivityController(IActivityService activityService) : ControllerBase
{
    [HttpGet("{id}")]
    public async Task<ActionResult> GetActivityById(Guid id)
    {
        var result = await activityService.GetActivityByIdAsync(id);
        return result.IsSuccess ? Ok(result.Value!) : NotFound(result.ErrorMessage!);
    }
    
    [HttpPost]
    public async Task<ActionResult> CreateActivity([FromBody] CreateActivityDto createActivityDto)
    {
        var result = await activityService.CreateActivityAsync(createActivityDto);
        return result.IsSuccess
            ? CreatedAtAction(nameof(GetActivityById), new { id = result.Value!.Id }, result.Value!)
            : BadRequest(result.ErrorMessage!);
    }
    
    [HttpGet("class/{classId}")]
    public async Task<ActionResult> GetActivitiesInClass(Guid classId) 
    {
        var activities = await activityService.GetActivitiesInClassAsync(classId);
        return Ok(activities);
    }

    [HttpGet("class/{classId}/parent-view/{studentId}")]
    public async Task<ActionResult> GetActivitiesInClassForParentView(Guid classId, Guid studentId)
    {
        var activities = await activityService.GetParentsActivityAsync(classId, studentId);
        return Ok(activities);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateActivity(Guid id, [FromBody] UpdateActivityDto updateActivityDto)
    {
        var result = await activityService.UpdateActivityAsync(id, updateActivityDto);
        return result.IsSuccess ? NoContent() : BadRequest(result.ErrorMessage!);
    }
    
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteActivity(Guid id)
    {
        var result = await activityService.DeleteActivityAsync(id);
        return result.IsSuccess ? NoContent() : BadRequest(result.ErrorMessage!);
    }

    [HttpPost("participants")]
    public async Task<ActionResult> AddParticipant([FromBody] AddActivityParticipantDto addParticipantDto)
    {
        var result = await activityService.AddParticipantAsync(addParticipantDto);
        return result.IsSuccess ? NoContent() : BadRequest(result.ErrorMessage!);
    }

    [HttpPost("participants/batch")]
    public async Task<ActionResult> AddParticipants(
        [FromBody] IEnumerable<AddActivityParticipantDto> addParticipantDtos)
    {
        var result = await activityService.AddParticipantsAsync(addParticipantDtos);
        return result.IsSuccess ? NoContent() : BadRequest(result.ErrorMessage!);
    }

    [HttpPut("participants/{id}")]
    public async Task<ActionResult> UpdateParticipant(Guid id,
        [FromBody] UpdateActivityParticipantDto updateParticipantDto)
    {
        var result = await activityService.UpdateParticipantAsync(id, updateParticipantDto);
        return result.IsSuccess ? NoContent() : BadRequest(result.ErrorMessage!);
    }

    [HttpDelete("participants/{id}")]
    public async Task<ActionResult> RemoveParticipant(Guid id)
    {
        var result = await activityService.RemoveParticipantAsync(id);
        return result.IsSuccess ? NoContent() : BadRequest(result.ErrorMessage!);
    }


}