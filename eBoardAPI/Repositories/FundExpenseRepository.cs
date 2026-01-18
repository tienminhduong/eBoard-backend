using eBoardAPI.Common;
using eBoardAPI.Context;
using eBoardAPI.Entities;
using eBoardAPI.Interfaces.Repositories;

namespace eBoardAPI.Repositories
{
    public class FundExpenseRepository(AppDbContext dbContext) : IFundExpenseRepository
    {
        public async Task<Result<FundExpense?>> AddnNewAsync(FundExpense fundExpense)
        {
            try
            {
                var res = await dbContext.FundExpenses.AddAsync(fundExpense);
                return Result<FundExpense?>.Success(res.Entity);
            }
            catch (Exception ex)
            {
                return Result<FundExpense?>.Failure(ex.Message);
            }
        } 
    }
}
