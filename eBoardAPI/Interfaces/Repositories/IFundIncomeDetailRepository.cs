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
        Task<Result<IEnumerable<StudentFundIncomeSummary>>> GetAllFundIncomeDetailsByIdFundIncomeAsync(Guid fundIncomeId, int pageNumber = 1, int pageSize = 20);
        Task<Result<FundIncomeDetail>> AddAsync(FundIncomeDetail fundIncomeDetail);
        Task<Result> UpdateAsync(FundIncomeDetail fundIncomeDetail);
    }
}
