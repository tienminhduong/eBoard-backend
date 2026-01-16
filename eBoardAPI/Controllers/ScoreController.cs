using eBoardAPI.Interfaces.Services;
using eBoardAPI.Models.ScoreSheet;
using Microsoft.AspNetCore.Mvc;

namespace eBoardAPI.Controllers;

[ApiController]
[Route("api/score")]
public class ScoreController(IScoreService scoreService) : ControllerBase
{
    [HttpGet("{classId}/summary/{semester}")]
    public async Task<ActionResult<ClassScoreSummaryDto>> GetClassScoreSummary(Guid classId, int semester)
    {
        var result = await scoreService.GetClassScoreSummaryAsync(classId, semester);
        return Ok(result);
    }
    
    
}