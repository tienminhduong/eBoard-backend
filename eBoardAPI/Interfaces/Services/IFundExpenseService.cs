using eBoardAPI.Common;
using eBoardAPI.Models.FundExpense;

namespace eBoardAPI.Interfaces.Services
{
    public interface IFundExpenseService
    {
        Task<Result<FundExpenseDto?>> CreateNewFundExpenseAsync(Guid classId, FundExpenseCreateDto fundExpenseCreateDto);
        Task<Result<IEnumerable<FundExpenseDto>>> GetFundExpensesByClassId(Guid classId, int pageNumber, int pageSize, DateOnly? startDate, DateOnly? endDate);
    }
}
