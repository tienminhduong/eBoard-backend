using eBoardAPI.Common;
using eBoardAPI.Context;
using eBoardAPI.Entities;
using eBoardAPI.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq;

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

        public async Task<Result<IEnumerable<FundIncomeStudent>>> GetFundIncomeDetailsByClassAndStudentAsync(Guid classId, Guid studentId)
        {
            try
            {
                // check class and student exist
                var existClassAndStudent = await dbContext.InClasses
                    .AsNoTracking()
                    .AnyAsync(ic => ic.ClassId == classId && ic.StudentId == studentId);

                if(!existClassAndStudent)
                {
                    return Result<IEnumerable<FundIncomeStudent>>.Failure($"Class ID {classId} and Student ID {studentId} association does not exist.");
                }
                // Query to get ClassFund IDs for the given classId
                var classFundIdQuery = dbContext.ClassFunds
                    .AsNoTracking()
                    .Where(cf => cf.ClassId == classId)
                    .Select(cf => cf.Id);

                // Query fundIncomes in class funds
                var fundIncomesQuery = dbContext.FundIncomes
                    .AsNoTracking()
                    .Where(fi => classFundIdQuery.Contains(fi.ClassFundId));

                // Query to get fundIncomeDetail of student
                var fundIncomeDetailsQuery = dbContext.FundIncomeDetails
                    .AsNoTracking()
                    .Where(fid => fid.StudentId == studentId);


                var result = await (from fi in fundIncomesQuery
                             join fid in fundIncomeDetailsQuery
                             on fi.Id equals fid.FundIncomeId
                             into fidGroup
                             select new FundIncomeStudent
                             {
                                 Id = fi.Id,
                                 Title = fi.Title,
                                 ExpectedAmount = fi.AmountPerStudent,
                                 PaidAmount = fidGroup.Sum(x => (int?)x.ContributedAmount) ?? 0,
                                 EndDate = fi.EndDate,
                                 Description = fi.Description
                             }).ToListAsync();



                return Result<IEnumerable<FundIncomeStudent>>.Success(result);

            }
            catch (Exception ex)
            {
                return Result<IEnumerable<FundIncomeStudent>>.Failure($"Error retrieving FundIncomeDetails for Class ID {classId} and Student ID {studentId}: {ex.Message}");
            }
        }
    }
}

public class FundIncomeStudent
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public int ExpectedAmount { get; set; }
    public int PaidAmount { get; set; }
    public DateOnly EndDate { get; set; }
    public string Description { get; set; } = string.Empty;
}