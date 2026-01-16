using eBoardAPI.Common;
using eBoardAPI.Entities;

namespace eBoardAPI.Interfaces.Repositories
{
    public interface IFundIncomeRepository
    {
        Task<Result<FundIncome>> AddAsync(FundIncome fundIncome);
        Task<Result<FundIncome>> GetByIdAsync(Guid id);
        Task<Result> UpdateAsync(FundIncome fundIncome);
    }
}
