using eBoardAPI.Common;
using eBoardAPI.Context;
using eBoardAPI.Entities;
using eBoardAPI.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace eBoardAPI.Repositories
{
    public class FundIncomeDetailRepository(AppDbContext dbContext) : IFundIncomeDetailRepository
    {
        public async Task<Result<FundIncomeDetail?>> GetFundIncomeDetailByIdAsync(Guid incomeDetailId)
        {
            try
            {
                var incomeDetail = await dbContext.FundIncomeDetails
                    .AsNoTracking()
                    .FirstOrDefaultAsync(fid => fid.Id == incomeDetailId);
                return Result<FundIncomeDetail?>.Success(incomeDetail);
            }
            catch (Exception ex)
            {
                return Result<FundIncomeDetail?>.Failure($"Error retrieving FundIncomeDetail with ID {incomeDetailId}: {ex.Message}");
            }
        }
    }
}
