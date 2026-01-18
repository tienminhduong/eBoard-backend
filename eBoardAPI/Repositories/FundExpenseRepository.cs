using eBoardAPI.Common;
using eBoardAPI.Context;
using eBoardAPI.Entities;
using eBoardAPI.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

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

        public async Task<Result<IEnumerable<FundExpense>>> GetAllByClassIdAsync(Guid classId, int pageNumber, int pageSize, DateOnly? startDate, DateOnly? endDate)
        {
            try
            {
                // get class fund id
                var classFundIdQuery = dbContext.ClassFunds
                    .AsNoTracking()
                    .Where(cf => cf.ClassId == classId)
                    .Select(cf => cf.Id);
                // get fund expense by class fund id
                var fundExpenseQuery = dbContext.FundExpenses
                    .AsNoTracking()
                    .Where(fe => classFundIdQuery.Contains(fe.ClassFundId));

                // filter by date range
                if (startDate != null)
                {
                    fundExpenseQuery = fundExpenseQuery.Where(fe => fe.ExpenseDate >= startDate);
                }
                if (endDate != null)
                {
                    fundExpenseQuery = fundExpenseQuery.Where(fe => fe.ExpenseDate <= endDate);
                }
                //order by expense date
                fundExpenseQuery = fundExpenseQuery.OrderByDescending(fe => fe.ExpenseDate);
                // pagination
                fundExpenseQuery = fundExpenseQuery
                    .Skip((pageNumber - 1) * pageSize);

                var result = await fundExpenseQuery.ToListAsync();
                return Result<IEnumerable<FundExpense>>.Success(result);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<FundExpense>>.Failure(ex.Message);
            }
        }
    }
}
