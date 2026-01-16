using eBoardAPI.Common;
using eBoardAPI.Models.FundIncome;

namespace eBoardAPI.Interfaces.Services
{
    public interface IFundIncomeService
    {
        Task<Result<FundIncomeDto>> CreateFundIncomeAsync(Guid classFundId, CreateFundIncomeDto fundIncome);
        Task<Result<FundIncomeDto>> GetFundIncomeByIdAsync(Guid id);
    }
}
