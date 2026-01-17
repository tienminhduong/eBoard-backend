using eBoardAPI.Common;
using eBoardAPI.Entities;
using eBoardAPI.Models.FundIncome;

namespace eBoardAPI.Interfaces.Repositories
{
    public interface IFundIncomeDetailRepository
    {
        Task<Result<FundIncomeDetail?>> GetFundIncomeDetailByIdAsync(Guid incomeDetailId);
        Task<Result<IEnumerable<FundIncomeStudent>>> GetFundIncomeDetailsByClassAndStudentAsync(Guid classId, Guid studentId);
        Task<Result<IEnumerable<FundIncomeDetail>>> GetFundIncomeDetailsByIncomeIdAndStudentIdAsync(Guid incomeId, Guid studentId);
        Task<Result<IEnumerable<StudentFundIncomeSummary>>> GetAllFundIncomeDetailsByIdFundIncomeAsync(Guid fundIncomeId);
    }
}
