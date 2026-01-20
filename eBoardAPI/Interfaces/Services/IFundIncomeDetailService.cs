using eBoardAPI.Common;
using eBoardAPI.Models.FundIncome;

namespace eBoardAPI.Interfaces.Services
{
    public interface IFundIncomeDetailService
    {
        Task<Result<FundIncomeDetailDto?>> GetFundIncomeDetailByIdAsync(Guid incomeDetailId);
        Task<Result<IEnumerable<FundIncomeStudent>>> GetFundIncomeDetailsByClassAndStudentAsync(Guid classId, Guid studentId);
        Task<Result<IEnumerable<FundIncomeDetailDto>>> GetFundIncomeDetailsByIncomeIdAndStudentIdAsync(Guid incomeId, Guid studentId);
        Task<Result<IEnumerable<StudentFundIncomeSummary>>> GetAllFundIncomeDetailsByIdFundIncomeAsync(Guid fundIncomeId);
        Task<Result<FundIncomeDetailDto>> CreateContributeFundIncome(Guid fundIncomeId, ContributeFundIncomeDto contributeFund);
        Task<Result> UpdateFundIncomeDetailAsync(Guid incomeDetailId, UpdateFundIncomeDetailDto updateFundIncomeDetail);
    }
}
