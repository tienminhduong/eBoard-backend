using eBoardAPI.Common;
using eBoardAPI.Entities;
using eBoardAPI.Migrations;

namespace eBoardAPI.Interfaces.Repositories
{
    public interface IFundExpenseRepository
    {
        Task<Result<FundExpense?>> AddnNewAsync(FundExpense fundExpense);
        Task<Result<IEnumerable<FundExpense>>> GetAllByClassIdAsync(Guid classId, 
                                                                    int pageNumber, 
                                                                    int pageSize, 
                                                                    DateOnly? startDate, 
                                                                    DateOnly? endDate);
    }
}
