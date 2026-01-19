using eBoardAPI.Common;
using eBoardAPI.Context;
using eBoardAPI.Entities;
using eBoardAPI.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

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

        public async Task<Result<IEnumerable<FundIncome>>> GetAllByClassIdAsync(Guid classId, 
                                                                 int pageNumber,
                                                                 int pageSize)
        {
            try
            {
                var classFundQuery = dbContext.ClassFunds
                                          .AsNoTracking()
                                          .Where(c => c.ClassId == classId);

                var query = dbContext.FundIncomes
                    .AsNoTracking()
                    .Join(classFundQuery,
                          fi => fi.ClassFundId,
                          c => c.Id,
                          (fi, c) => fi)
                    .OrderByDescending(fi => fi.ExpectedAmount - fi.CollectedAmount)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize);

                var fundIncomes = await query.ToListAsync();
                return Result<IEnumerable<FundIncome>>.Success(fundIncomes);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<FundIncome>>.Failure($"An error occurred while retrieving FundIncomes: {ex.Message}");
            }
        }

        public async Task<Result<FundIncome>> GetByIdAsync(Guid id)
        {
            try
            {
                var fundIncome = await dbContext.FundIncomes.FindAsync(id);
                return Result<FundIncome>.Success(fundIncome);
            }
            catch (Exception ex)
            {
                return Result<FundIncome>.Failure($"An error occurred while retrieving FundIncome: {ex.Message}");
            }
        }

        public async Task<Result> UpdateAsync(FundIncome fundIncome)
        {
            dbContext.FundIncomes.Update(fundIncome);
            await dbContext.SaveChangesAsync();
            return Result.Success();
        }

        public async Task<Result<IEnumerable<FundIncomeDetail>>> GetFundIncomeDetailsByStudentIdAsync(Guid studentId)
        {
            try
            {
                var fundIncomeDetails = await dbContext.FundIncomeDetails
                    .AsNoTracking()
                    .Where(fid => fid.StudentId == studentId)
                    .ToListAsync();
                return Result<IEnumerable<FundIncomeDetail>>.Success(fundIncomeDetails);

            }
            catch (Exception ex)
            {
                return Result<IEnumerable<FundIncomeDetail>>.Failure($"An error occurred while retrieving FundIncomeDetails: {ex.Message}");
            }
        }
    }
}
