using eBoardAPI.Common;
using eBoardAPI.Context;
using eBoardAPI.Entities;
using eBoardAPI.Interfaces.Repositories;

namespace eBoardAPI.Repositories
{
    public class FundIncomeRepository(AppDbContext dbContext) : IFundIncomeRepository
    {
        public async Task<Result<FundIncome>> AddAsync(FundIncome fundIncome)
        {
            try
            {
                await dbContext.FundIncomes.AddAsync(fundIncome);
                await dbContext.SaveChangesAsync();
                return Result<FundIncome>.Success(fundIncome);
            }
            catch (Exception ex)
            {
                return Result<FundIncome>.Failure($"An error occurred while adding FundIncome: {ex.Message}");
            }
        }

        public async Task<Result<FundIncome>> GetByIdAsync(Guid id)
        {
            var fundIncome = await dbContext.FundIncomes.FindAsync(id);

            if (fundIncome == null)
            {
                return Result<FundIncome>.Failure("FundIncome not found.");
            }
            return Result<FundIncome>.Success(fundIncome);
        }

        public async Task<Result> UpdateAsync(FundIncome fundIncome)
        {
            dbContext.FundIncomes.Update(fundIncome);
            await dbContext.SaveChangesAsync();
            return Result.Success();
        }
    }
}
