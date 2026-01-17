using eBoardAPI.Common;
using eBoardAPI.Context;
using eBoardAPI.Entities;
using eBoardAPI.Interfaces.Repositories;
using eBoardAPI.Models.FundIncome;
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

        public async Task<Result<IEnumerable<FundIncomeDetail>>> GetFundIncomeDetailsByIncomeIdAndStudentIdAsync(Guid incomeId, Guid studentId)
        {
            try
            {
                var fundIncomeDetails = await dbContext.FundIncomeDetails
                    .AsNoTracking()
                    .Where(fid => fid.FundIncomeId == incomeId && fid.StudentId == studentId)
                    .Include(fid => fid.FundIncome)
                    .ToListAsync();
                return Result<IEnumerable<FundIncomeDetail>>.Success(fundIncomeDetails);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<FundIncomeDetail>>.Failure($"Error retrieving FundIncomeDetails for Income ID {incomeId} and Student ID {studentId}: {ex.Message}");
            }
        }

        void PrintList(IEnumerable<object> list)
        {
            foreach (var item in list)
            {
                Console.WriteLine(item);
            }
        }
        public async Task<Result<IEnumerable<StudentFundIncomeSummary>>> GetAllFundIncomeDetailsByIdFundIncomeAsync(Guid fundIncomeId)
        {
            try
            {
                //get fund income
                var fundIncomeInfoQuery =
                                from fi in dbContext.FundIncomes.AsNoTracking()
                                join cf in dbContext.ClassFunds.AsNoTracking()
                                    on fi.ClassFundId equals cf.Id
                                where fi.Id == fundIncomeId
                                select new
                                {
                                    FundIncomeId = fi.Id,
                                    ClassId = cf.ClassId,
                                    fi.ExpectedAmount
                                };
                //lay tat ca hoc sinh trong lop
                var studentsInClassQuery =
                                from f in fundIncomeInfoQuery
                                join ic in dbContext.InClasses.AsNoTracking()
                                    on f.ClassId equals ic.ClassId
                                join s in dbContext.Students.AsNoTracking()
                                    on ic.StudentId equals s.Id
                                select new
                                {
                                    StudentId = s.Id,
                                    FullName = s.LastName + " " + s.FirstName,
                                    f.FundIncomeId,
                                    f.ExpectedAmount
                                };
                
                var t = await studentsInClassQuery.ToListAsync();
                PrintList(t);
                // group fund income detail theo student
                var fundDetailAggQuery =
                                from fd in dbContext.FundIncomeDetails.AsNoTracking()
                                where fd.FundIncomeId == fundIncomeId
                                group fd by fd.StudentId into g
                                select new
                                {
                                    StudentId = g.Key,
                                    TotalContributedAmount = (int?)g.Sum(x => x.ContributedAmount),
                                    LatestContributedAt = g.Max(x => (DateOnly?)x.ContributedAt)
                                };
                
                var s2 = await fundDetailAggQuery.ToListAsync();
                PrintList(s2);
                // left join va lay ket qua
                var result =
                             from s in studentsInClassQuery

                             join agg in fundDetailAggQuery
                                 on s.StudentId equals agg.StudentId into aggGroup
                             from agg in aggGroup.DefaultIfEmpty()

                             select new StudentFundIncomeSummary
                             {
                                 StudentId = s.StudentId,
                                 FullName = s.FullName,
                                 ExpectedAmount = s.ExpectedAmount,

                                 TotalContributedAmount = agg.TotalContributedAmount ?? 0,

                                 LatestContributedAt = agg.LatestContributedAt ?? null,

                                 LatestNotes = agg.LatestContributedAt != null
                                 ? dbContext.FundIncomeDetails
                                     .Where(fd =>
                                         fd.StudentId == s.StudentId &&
                                         fd.FundIncomeId == fundIncomeId &&
                                         fd.ContributedAt == agg.LatestContributedAt
                                     )
                                     .OrderByDescending(fd => fd.Id)
                                     .Select(fd => fd.Notes)
                                     .FirstOrDefault() ?? string.Empty
                                 : string.Empty
                             };


                var data = await result
                    .AsNoTracking()
                    .ToListAsync();
                return Result<IEnumerable<StudentFundIncomeSummary>>.Success(data);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<StudentFundIncomeSummary>>.Failure($"Error retrieving FundIncomeDetails for Fund Income ID {fundIncomeId}: {ex.Message}");
            }
        }
    }
}