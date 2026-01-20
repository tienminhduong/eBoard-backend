using eBoardAPI.Interfaces.Services;
using eBoardAPI.Models.ClassFund;
using eBoardAPI.Models.FundExpense;
using eBoardAPI.Models.FundIncome;
using Microsoft.AspNetCore.Mvc;

namespace eBoardAPI.Controllers;

[ApiController]
[Route("api/funds")]
public class ClassFundController(IClassFundService classFundService,
                                 IFundIncomeService fundIncomeService,
                                 IFundIncomeDetailService fundIncomeDetailService,
                                 IFundExpenseService fundExpenseService) : ControllerBase
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
        var result = await fundExpenseService.GetFundExpensesByClassId(classId,  pageNumber, pageSize, startDate, endDate);
        if (!result.IsSuccess)
        {
            return BadRequest(result.ErrorMessage);
        }
        return (result.Value!.Any()) ? Ok(result.Value) : NotFound();
    }

    [HttpPost("{classId}/expenses")]
    public async Task<ActionResult> AddNewFundExpense(Guid classId, [FromBody] FundExpenseCreateDto expenseRecord)
    {
        var result = await fundExpenseService.CreateNewFundExpenseAsync(classId, expenseRecord);
        if(!result.IsSuccess) {
            return BadRequest(result.ErrorMessage);
        }
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

    [HttpPost("contributions/{fundIncomeId}")]
    public async Task<ActionResult> ContributeFundIncome(Guid fundIncomeId, [FromBody]ContributeFundIncomeDto contributeFund)
    {
        var result = await fundIncomeDetailService.CreateContributeFundIncome(fundIncomeId, contributeFund);
        if(!result.IsSuccess)
        {
            return BadRequest(result.ErrorMessage);
        }
        return CreatedAtAction(nameof(GetFundIncomeDetailsById), new { incomeDetailId = fundIncomeId }, result.Value);
    }

    [HttpGet("incomes/{fundIncomeId}")]
    public async Task<ActionResult> GetFundIncomeById(Guid fundIncomeId)
    {
        var result = await fundIncomeService.GetFundIncomeByIdAsync(fundIncomeId);
        if(!result.IsSuccess)
        {
            return BadRequest(result.ErrorMessage);
        }
        return (result.Value != null) ? Ok(result.Value) : NotFound();
    }

    [HttpPut("incomes/{fundIncomeId}")]
    public async Task<ActionResult> UpdateFundIncomeById(Guid fundIncomeId, [FromBody] UpdateFundIncomeDto updateFundIncome)
    {
        var result = await fundIncomeService.UpdateFundIncomeAsync(fundIncomeId, updateFundIncome);
        if(!result.IsSuccess)
        {
            return BadRequest(result.ErrorMessage);
        }
        return NoContent();
    }

    [HttpPut("income-details/{incomeDetailId}")]
    public async Task<ActionResult> UpdateFundIncomeDetailById(Guid incomeDetailId, [FromBody] UpdateFundIncomeDetailDto updateFundIncomeDetail)
    {
        var result = await fundIncomeDetailService.UpdateFundIncomeDetailAsync(incomeDetailId, updateFundIncomeDetail);
        if(!result.IsSuccess)
        {
            return BadRequest(result.ErrorMessage);
        }
        return NoContent();
    }
}