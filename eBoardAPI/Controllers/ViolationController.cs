using eBoardAPI.Interfaces.Services;
using eBoardAPI.Models.Violation;
using Microsoft.AspNetCore.Mvc;

namespace eBoardAPI.Controllers
{
    [ApiController]
    [Route("api")]
    public class ViolationController(IViolationService violationService) : ControllerBase
    {
        [HttpPost("violations")]
        public async Task<ActionResult> CreateViolation([FromBody] CreateViolationDto createViolationDto)
        {
            // Implementation for creating a violation
            var result = await violationService.CreateNewViolation(createViolationDto);
            if(!result.IsSuccess)
            {
                return BadRequest(result.ErrorMessage);
            }    
            return Created();
        }

        [HttpGet("violations/{violationId}")]
        public async Task<ActionResult> GetViolationById(Guid violationId)
        {
            // Implementation for retrieving a violation by ID
            var result = await violationService.GetViolationById(violationId);
            if(!result.IsSuccess)
            {
                return BadRequest(result.ErrorMessage);
            }    
            var violation = result.Value;
            return violation is null ? NotFound() : Ok(violation);
        }

        [HttpPut("violations/{violationId}")]
        public async Task<ActionResult> UpdateViolation(Guid violationId, UpdateViolationDto updateViolationDto)
        {
            // Implementation for updating a violation
            var result = await violationService.UpdateViolation(violationId, updateViolationDto);
            if(!result.IsSuccess)
            {
                return BadRequest(result.ErrorMessage);
            }    
            return Ok(result.Value);
        }

        [HttpGet("/classes/{classId}/violations")]
        public async Task<ActionResult> GetViolationsByClass(Guid classId)
        {
            // Implementation for retrieving violations by class
            var result = await violationService.GetViolationsByClassId(classId);
            if(!result.IsSuccess)
            {
                return BadRequest(result.ErrorMessage);
            }
            var violations = result.Value!;
            return violations.Any() ? Ok(result.Value) : NotFound();
        }

        [HttpGet("/classes/{classId}/violations/stats")]
        public async Task<ActionResult> GetViolationStatsByClass(Guid classId)
        {
            var result = await violationService.GetViolationStatsByClassId(classId);
            if(!result.IsSuccess)
            {
                return BadRequest(result.ErrorMessage);
            }
            return Ok(result.Value);
        }

        [HttpGet("classes/{classId}/students/{studentId}/violations")]
        public async Task<ActionResult>GetViolationsByClassIdAndStudentId(Guid classId, Guid studentId)
        {
            var result = await violationService.GetViolationsByClassIdAndStudentId(classId, studentId);
            if(!result.IsSuccess)
            {
                return BadRequest(result.ErrorMessage);
            }
            var violations = result.Value!;
            return violations.Any() ? Ok(violations) : NotFound();
        }
    }
}
