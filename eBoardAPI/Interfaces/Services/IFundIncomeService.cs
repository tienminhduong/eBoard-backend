using eBoardAPI.Common;
using eBoardAPI.Models.FundIncome;

namespace eBoardAPI.Interfaces.Services
{
    public interface IFundIncomeService
    {
        Task<Result<FundIncomeDto>> CreateFundIncomeAsync(Guid classId, CreateFundIncomeDto fundIncome);
        Task<Result<FundIncomeDto>> GetFundIncomeByIdAsync(Guid id);
        Task<Result<IEnumerable<FundIncomeDto>>> GetFundIncomesByClassIdAsync(Guid classId, int pageNumber, int pageSize);
        Task<Result<IEnumerable<FundIncomeDetailDto>>> GetFundIncomeDetailsByStudentIdAsync(Guid studentId);
        Task<Result<FundIncomeDto>> UpdateFundIncomeAsync(Guid id, UpdateFundIncomeDto updatedFundIncome);
    }
}
