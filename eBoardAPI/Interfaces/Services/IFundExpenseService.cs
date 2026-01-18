using eBoardAPI.Common;
using eBoardAPI.Models.FundExpense;

namespace eBoardAPI.Interfaces.Services
{
    public interface IFundExpenseService
    {
        Task<Result<FundExpenseDto?>> CreateNewFundExpenseAsync(Guid classId, FundExpenseCreateDto fundExpenseCreateDto);
    }
}
