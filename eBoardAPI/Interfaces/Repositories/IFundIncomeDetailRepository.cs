using eBoardAPI.Common;
using eBoardAPI.Entities;
using eBoardAPI.Models.FundIncome;

namespace eBoardAPI.Interfaces.Repositories
{
    public interface IFundIncomeDetailRepository
    {
        Task<Result<FundIncomeDetail?>> GetFundIncomeDetailByIdAsync(Guid incomeDetailId);
    }
}
