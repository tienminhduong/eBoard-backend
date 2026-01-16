using eBoardAPI.Common;
using eBoardAPI.Entities;
using Microsoft.IdentityModel.Tokens;
using System.Linq.Expressions;

namespace eBoardAPI.Interfaces.Repositories
{
    public interface IFundIncomeRepository
    {
        Task<Result<FundIncome>> AddAsync(FundIncome fundIncome);
        Task<Result<FundIncome>> GetByIdAsync(Guid id);
        Task<Result> UpdateAsync(FundIncome fundIncome);
        Task<Result<IEnumerable<FundIncome>>> GetAllByClassIdAsync(Guid classId, 
                                                          int pageNumber, 
                                                          int pageSize);
    }
}
