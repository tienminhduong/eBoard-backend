using eBoardAPI.Common;
using eBoardAPI.Models.FundIncome;

namespace eBoardAPI.Interfaces.Services
{
    public interface IFundIncomeDetailService
    {
        Task<Result<FundIncomeDetailDto?>> GetFundIncomeDetailByIdAsync(Guid incomeDetailId);
    }
}
