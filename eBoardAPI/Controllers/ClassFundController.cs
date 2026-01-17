using eBoardAPI.Interfaces.Services;
using eBoardAPI.Models.ClassFund;
using eBoardAPI.Models.FundIncome;
using Microsoft.AspNetCore.Mvc;

namespace eBoardAPI.Controllers;

[ApiController]
[Route("api/funds")]
public class ClassFundController(IClassFundService classFundService,
                                 IFundIncomeService fundIncomeService,
                                 IFundIncomeDetailService fundIncomeDetailService) : ControllerBase
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
        if(ModelState.IsValid == false)
        {
            return BadRequest(ModelState);
        }
        
        var result = await fundIncomeService.GetFundIncomesByClassIdAsync(classId, pageNumber, pageSize);
        if (!result.IsSuccess)
        {
            return BadRequest(result.ErrorMessage);
        }
        return (result.Value!.Any()) ? Ok(result.Value) : NotFound();
    }
    
    [HttpPost("{classId}/income")]
    public async Task<ActionResult> AddNewFundIncome(Guid classId, [FromBody] CreateFundIncomeDto fundRecord)
    {
        if(ModelState.IsValid == false)
        {
            return BadRequest(ModelState);
        }

        var validationError = fundRecord.ValidateData();
        if(validationError != string.Empty)
        {
            return BadRequest(validationError);
        }

        var res = await fundIncomeService.CreateFundIncomeAsync(classId, fundRecord);
        return res.IsSuccess ? CreatedAtAction(nameof(GetClassFundByClassId), new { classId = classId }, res.Value)
                            : BadRequest(res.ErrorMessage);
    }

    [HttpGet("income/{incomeDetailId}/details")]
    public async Task<ActionResult> GetFundIncomeDetailsById(Guid incomeDetailId)
    {
        var result = await fundIncomeDetailService.GetFundIncomeDetailByIdAsync(incomeDetailId);
        if (!result.IsSuccess)
        {
            return BadRequest(result.ErrorMessage);
        }
        return (result.Value != null) ? Ok(result.Value) : NotFound();
    }

    [HttpGet("income/{incomeId}/details/{studentId}")]
    public async Task<ActionResult> GetFundIncomeDetailByStudent(Guid incomeId, Guid studentId)
    {
        if(ModelState.IsValid == false)
        {
            return BadRequest(ModelState);
        }
        var result = await fundIncomeDetailService.GetFundIncomeDetailsByIncomeIdAndStudentIdAsync(incomeId, studentId);
        if (!result.IsSuccess)
        {
            return BadRequest(result.ErrorMessage);
        }
        return (result.Value!.Any()) ? Ok(result.Value) : NotFound();
    }

    [HttpGet("income/{studentId}")]
    public async Task<ActionResult> GetFundIncomeByStudent(Guid studentId)
    {
        if(ModelState.IsValid == false)
        {
            return BadRequest(ModelState);
        }
        var result = await fundIncomeService.GetFundIncomeDetailsByStudentIdAsync(studentId);
        if(!result.IsSuccess)
        {
            return BadRequest(result.ErrorMessage);
        }
        return (result.Value!.Any()) ? Ok(result.Value) : NotFound();
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

    [HttpGet("classes/{classId}/students/{studentId}/income-details")]
    public async Task<ActionResult> GetIncomeDetailsByClassAndStudent(Guid classId, Guid studentId)
    {
        if(ModelState.IsValid == false)
        {
            return BadRequest(ModelState);
        }
        //await Task.Delay(1); // Simulate async operation;
        var result = await fundIncomeDetailService.GetFundIncomeDetailsByClassAndStudentAsync(classId, studentId);
        if (!result.IsSuccess)
        {
            return BadRequest(result.ErrorMessage);
        }
        return (result.Value!.Any()) ? Ok(result.Value) : NotFound();
    }

    [HttpGet("{fundIncomeId}/summary")]
    public async Task<ActionResult> GetSummaryFundIncomeByIdAsync(Guid fundIncomeId)
    {
        var result = await fundIncomeDetailService.GetAllFundIncomeDetailsByIdFundIncomeAsync(fundIncomeId);
        if(!result.IsSuccess)
        {
            return BadRequest(result.ErrorMessage);
        }
        return (result.Value!.Any()) ? Ok(result.Value) : NotFound();
    }
}