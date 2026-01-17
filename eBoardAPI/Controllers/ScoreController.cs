using eBoardAPI.Interfaces.Services;
using eBoardAPI.Models.ScoreSheet;
using eBoardAPI.Models.Subject;
using Microsoft.AspNetCore.Mvc;

namespace eBoardAPI.Controllers;

[ApiController]
[Route("api/score")]
public class ScoreController(IScoreService scoreService) : ControllerBase
{
    [HttpGet("{classId}/subjects")]
    public async Task<ActionResult<IEnumerable<SubjectDto>>> GetClassSubjects(Guid classId)
    {
        var result = await scoreService.GetClassSubjectsAsync(classId);
        return Ok(result);
    }
    
    [HttpGet("{classId}/summary/{semester}")]
    public async Task<ActionResult<ClassScoreSummaryDto>> GetClassScoreSummary(Guid classId, int semester)
    {
        var result = await scoreService.GetClassScoreSummaryAsync(classId, semester);
        return Ok(result);
    }

    [HttpGet("{classId}/student/{studentId}/scores/{semester}")]
    public async Task<ActionResult<StudentScoreSheetDto>> GetStudentScoreSheet(Guid classId, Guid studentId, int semester)
    {
        var result = await scoreService.GetStudentScoreSheetAsync(classId, studentId, semester);
        if (!result.IsSuccess)
            return NotFound(result.ErrorMessage);
        return Ok(result.Value);
    }
    
    [HttpGet("{classId}/subject/{subjectId}/scores/{semester}")]
    public async Task<ActionResult<IEnumerable<StudentScoreBySubjectDto>>> GetStudentScoresBySubject(Guid classId, Guid subjectId, int semester)
    {
        var result = await scoreService.GetStudentScoreBySubjectsAsync(classId, subjectId, semester);
        return Ok(result);
    }

    [HttpPut("{classId}/student/{studentId}/scores/{semester}")]
    public async Task<ActionResult> UpdateStudentScores(Guid classId, Guid studentId, int semester,
        [FromBody] UpdateIndividualStudentScoreSheetDto updateDto)
    {
        var (result, isCreated) =
            await scoreService.AddOrUpdateStudentScoreSheetAsync(classId, studentId, semester, updateDto);
        
        if (!result.IsSuccess)
            return BadRequest(result.ErrorMessage);
        
        if (isCreated)
            return CreatedAtAction(nameof(GetStudentScoreSheet), new { classId, studentId, semester }, result.Value);
        
        return Ok(result.Value);
    }

    [HttpPut("{classId}/subject/{subjectId}/scores/{semester}")]
    public async Task<ActionResult> UpdateScoresBySubject(Guid classId, Guid subjectId, int semester,
        [FromBody] IEnumerable<UpdateStudentScoreBySubjectDto> updateDtos)
    {
        var isSuccess = await scoreService.UpdateScoresBySubjectAsync(classId, subjectId, semester, updateDtos);
        if (!isSuccess)
            return BadRequest("Cập nhật điểm thất bại");
        return NoContent();
    }
}