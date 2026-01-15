using eBoardAPI.Interfaces.Services;
using eBoardAPI.Models.ClassFund;
using Microsoft.AspNetCore.Mvc;

namespace eBoardAPI.Controllers;

[ApiController]
[Route("api/funds")]
public class ClassFundController(IClassFundService classFundService) : ControllerBase
{
    //authorize as teacher and parent
    [HttpGet("{classId}")]
    public async Task<ActionResult<ClassFundDto>> GetClassFundByClassId(Guid classId)
    {
        var result = await classFundService.GetClassFundByClassIdAsync(classId);
        return result.IsSuccess
            ? Ok(result.Value)
            : NotFound(result.ErrorMessage);
    }
    
    [HttpGet("{classId}/income")]
    public async Task<ActionResult> GetFundIncomeByClassId(Guid classId, int pageNumber = 1, int pageSize = 20)
    {
        await Task.Delay(1); // Simulate async operation;
        return Ok();
    }
    
    [HttpPost("{classId}/income")]
    public async Task<ActionResult> AddNewFundIncome(Guid classId, [FromBody] object fundRecord)
    {
        await Task.Delay(1); // Simulate async operation;
        return CreatedAtAction(nameof(GetClassFundByClassId), new { classId = classId }, null);
    }

    [HttpGet("income/{incomeId}/details")]
    public async Task<ActionResult> GetFundIncomeDetailsById(Guid incomeId)
    {
        await Task.Delay(1); // Simulate async operation;
        return Ok();
    }

    [HttpGet("income/{incomeId}/details/{studentId}")]
    public async Task<ActionResult> GetFundIncomeDetailByStudent(Guid incomeId, Guid studentId)
    {
        await Task.Delay(1); // Simulate async operation;
        return Ok();
    }

    [HttpGet("income/{studentId}")]
    public async Task<ActionResult> GetFundIncomeByStudent(Guid studentId)
    {
        await Task.Delay(1); // Simulate async operation;
        return Ok();
    }

    [HttpGet("{classId}/expenses")]
    public async Task<ActionResult> GetFundExpensesByClassId(Guid classId, int pageNumber = 1, int pageSize = 20,
        DateOnly? startDate = null, DateOnly? endDate = null)
    {
        await Task.Delay(1); // Simulate async operation;
        return Ok();
    }

    [HttpPost("{classId}/expenses")]
    public async Task<ActionResult> AddNewFundExpense(Guid classId, [FromBody] object expenseRecord)
    {
        await Task.Delay(1); // Simulate async operation;
        return CreatedAtAction(nameof(GetClassFundByClassId), new { classId = Guid.NewGuid() }, null);
    }
}